require 'json'

class Api::EnemiesController < ApplicationController
  def index
    hash = []
    Enemy.all.each do |enemy|
      hash << {
        hp: enemy.hp,
        mp: enemy.mp,
        id: enemy.id,
        speed: enemy.speed,
        power: enemy.power,
	skill: enemy.skill
      }
    end
    render json: hash 
  end

  def actions
    @actions = Enemy.find_by(id: params[:enemy_id]).actions.all
    @actions.order(:execution)
    hash = []
    @actions.each do |action|
      hash << {
        id: action.id,
        character: action.character,
        parameter: action.parameter,
        value_lower: action.value,
        value_upper: action.comparison,
        action: action.action,
 	enemy_id: action.enemy_id
      }
    end
    render json: hash
  end
end
