class Tasks::Batch
  def self.execute
    p "Hello World"
  end
  
  def self.skill_change
    Enemy.all.each do |enemy|
      enemy.skill = 1
    end
  end
end
