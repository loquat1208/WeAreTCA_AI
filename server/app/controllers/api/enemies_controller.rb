require 'json'
class Api::EnemiesController < ApplicationController
  def index
    @enemies = Enemy.all
    render json: @enemies
  end
  
  def info
    @enemies = Enemy.all
    if !@enemies.nil?
      render json: @enemies
    end
  end
end
