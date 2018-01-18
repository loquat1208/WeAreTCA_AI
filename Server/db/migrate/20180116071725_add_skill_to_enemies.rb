class AddSkillToEnemies < ActiveRecord::Migration[5.1]
  def change
    add_column :enemies, :skill, :integer
  end
end
