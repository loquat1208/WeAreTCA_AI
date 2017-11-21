class RemovePersonalityFromEnemy < ActiveRecord::Migration[5.1]
  def change
    remove_column :enemies, :personality, :integer
  end
end
