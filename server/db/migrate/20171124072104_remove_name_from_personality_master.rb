class RemoveNameFromPersonalityMaster < ActiveRecord::Migration[5.1]
  def change
    remove_column :personality_masters, :name, :string
  end
end
