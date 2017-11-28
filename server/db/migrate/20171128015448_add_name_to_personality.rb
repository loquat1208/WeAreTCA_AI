class AddNameToPersonality < ActiveRecord::Migration[5.1]
  def change
    add_column :personality_masters, :name, :string
  end
end
