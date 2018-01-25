class AddMpToEnemies < ActiveRecord::Migration[5.1]
  def change
    add_column :enemies, :mp, :integer
  end
end
