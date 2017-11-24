class CreatePersonalityMasters < ActiveRecord::Migration[5.1]
  def change
    create_table :personality_masters do |t|
      t.integer :code
      t.string :name

      t.timestamps
    end
  end
end
