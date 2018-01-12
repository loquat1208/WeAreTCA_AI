require 'json'

class Api::EnemiesController < ApplicationController
  def index
    render json: Enemy.all
  end

  def actions
    @actions = Enemy.find_by(id: params[:enemy_id]).actions.all
    render json: @actions
  end
end
