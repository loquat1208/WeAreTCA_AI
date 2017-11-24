require 'csv'

module LoadCSV
class Seeds
  ##############################################################################
  # runner 用
  ##############################################################################
  class << self
    def run
      LoadCSV::Seeds.new.run
    end
  end
  ##############################################################################
  # import 用定数
  ##############################################################################

  MASTER_DIR = Rails.root.join("db", "masters", "csv")
  
  ATTRS = {
    personality_masters: [:code, :name]
  }

  def run
    reset_bulk_insert(:personality_masters)
  end
  ##############################################################################
  # Core methods
  ##############################################################################

  def reset_bulk_insert(name, attrs=nil, csv_name=nil)
    reset(name) do
      bulk_insert_from_csv(name, attrs, csv_name)
    end
  end

  def reset(*names)
    names = names.map(&:to_s).map(&:pluralize)
    ActiveRecord::Base.transaction do
      reset_table(*names)
      yield if block_given?
    end
  end

  def reset_table(*names)
    names.flatten.each do |name|
      clazz = name.to_s.classify.constantize
      talbe_name = clazz.name.underscore.pluralize
      clazz.connection.execute("TRUNCATE TABLE #{talbe_name};")
      puts "#{talbe_name} table truncated!"
    end
  end

  def bulk_insert_from_csv(name, attrs = nil, csv_name = nil)
    attrs = ATTRS[name] if attrs.nil?
    raise "nil attrs - #{name}" if attrs.nil?

    print "importing #{name}............"
    models = build_from_csv(name, attrs, csv_name)
    bulk_insert(models)
    puts "DONE!"
  end

  def each_line_from_csv(name)
    filename = File.join(MASTER_DIR, "#{name.to_s.pluralize}.csv")
    filename = File.join(MASTER_DIR, "#{name.to_s.gsub(/_masters/, "")}.csv") unless File.exist?(filename)

    CSV.foreach(filename, {:col_sep => ",", :headers => true}) do |line|
      line[0] =~ /^#.*/ ? next : line.delete_if{|h,f| h == '無視'}
      yield line if block_given?
    end
  end

  def create_from_csv(name, clazz=nil)
    print "importing #{name}....."
    clazz ||= name.to_s.classify.constantize
    each_line_from_csv(name) do |line|
      model = clazz.new
      yield(model, line) if block_given?
      model.save!
    end
    puts "DONE!"
  end

  def build_from_csv(name, attrs = [], csv_name=nil, &proc)
    models = []
    name = name.to_s
    if csv_name.blank?
      filename = name
    else
      filename = csv_name
    end
    clazz = name.classify.constantize
    tmp_model = clazz.new
    each_line_from_csv(filename) do |line|
      line = line.map{ |h, f| f.try(:strip).try(:gsub, /\r/, "") }
      print "CSV VALUE = #{line}"
      options = {}
      attrs.each_with_index do |attr, i|
        # NOTE: INSERT_ATTRSのカラムが本当にあるかチェックを入れた
        if attr && tmp_model.attributes.has_key?(attr.to_s)
          if line[i].blank?
            options[attr] = clazz.column_defaults[attr.to_s]
          else
            options[attr] = line[i]
          end
        end
      end
      model = clazz.new(options)

      yield(model, line) if block_given?
      models << model
    end
    models
  end

  def bulk_insert(records)
    return if records.blank?
    clazz = records[0].class
    attr_keys = records[0].attributes.keys
    attr_keys = attr_keys.join(",")

    now = Time.now
    attr_values = records.map { |record|
      record.created_at = now
      record.updated_at = now
      values = record.attributes.values
      clazz.send(:sanitize_sql, ["(?)", values])
    }.join(",")
    # 暫定のworkaround
    clazz.connection.execute('set sql_mode=""')
    sql = "INSERT INTO #{clazz.table_name}(#{attr_keys}) VALUES #{attr_values}"
    clazz.connection.execute(sql)
    # clazz.connection.execute(<<-SQL)
    #       INSERT INTO #{clazz.table_name}(#{attr_keys}) VALUES #{attr_values}
    #     SQL
    # wlog "-bluk inserted #{clazz.to_s}"
  end

  def str_to_boolean(str)
    return false if str.blank?
    return true if str.to_i == 1 || str.upcase == 'TRUE'
    return false
  end

  def comma_str_to_time(str)
    return nil if str.blank?
    return Time.mktime(*str.split(','))
  end

  def set_csv_values_to_model(model, line, attrs)
    attrs.each_with_index do |attr, i|
      # NOTE: INSERT_ATTRSのカラムが本当にあるかチェックを入れた
      if attr && model.attributes.has_key?(attr.to_s)
        model[attr] = line[i]
      end
    end
  end
end
end
