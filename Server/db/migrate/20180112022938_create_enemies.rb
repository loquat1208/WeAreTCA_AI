class CreateEnemies < ActiveRecord::Migration[5.1]
  def change
    create_table :enemies do |t|
      t.integer :power
      t.integer :speed
      t.integer :hp

      t.timestamps
    end
  end
end
