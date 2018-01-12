class CreateActions < ActiveRecord::Migration[5.1]
  def change
    create_table :actions do |t|
      t.integer :execution
      t.integer :character
      t.integer :parameter
      t.integer :value
      t.integer :comparison
      t.integer :action
      t.belongs_to :enemy, foreign_key: true

      t.timestamps
    end
  end
end
