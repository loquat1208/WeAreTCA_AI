class AddColumnToPersonalityMaster < ActiveRecord::Migration[5.1]
  def change
    add_column :personality_masters, :hp, :int
  end
end
