class CreateEnemies < ActiveRecord::Migration[5.1]
  def change
    create_table :enemies do |t|
      t.integer :personality
      t.float :power
      t.float :speed
      t.float :hp

      t.timestamps
    end
  end
end
