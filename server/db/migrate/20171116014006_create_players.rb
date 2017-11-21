class CreatePlayers < ActiveRecord::Migration[5.1]
  def change
    create_table :players do |t|
      t.float :power
      t.float :speed
      t.float :hp

      t.timestamps
    end
  end
end
