class Enemy < ApplicationRecord
  has_many :enemy_actions
  accepts_nested_attributes_for :enemy_actions, :allow_destroy => true
end
