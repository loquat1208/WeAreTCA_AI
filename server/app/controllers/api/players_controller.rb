require 'json'
class Api::PlayersController < ApplicationController
  def index
    @players = Player.all
    render json: @players
  end
  
  def info 
    @players = Player.all
    if !@players.nil?
      render json: @players
    end
  end
end
