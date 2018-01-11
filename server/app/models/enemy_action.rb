class EnemyAction < ApplicationRecord
  belongs_to :enemy, optional: true
end
