class CreateEnemyActions < ActiveRecord::Migration[5.1]
  def change
    create_table :enemy_actions do |t|
      t.belongs_to :enemy
      t.integer :executionOrder
      t.integer :character
      t.integer :parameter
      t.integer :value
      t.integer :comparision
      t.integer :action

      t.timestamps
    end
  end
end
