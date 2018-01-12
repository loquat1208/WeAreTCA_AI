class Enemy < ApplicationRecord
  has_many :actions, inverse_of: :enemy
  accepts_nested_attributes_for :actions, allow_destroy: true
end
