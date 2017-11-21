class AddPersonalityToEnemy < ActiveRecord::Migration[5.1]
  def change
    add_column :enemies, :personality, :integer
  end
end
