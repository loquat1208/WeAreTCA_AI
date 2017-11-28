require 'json'
require 'seeds'
class Api::MastersController < ApplicationController
  include LoadCSV
  def index
    LoadCSV::Seeds.run
    @personality_masters = PersonalityMaster.pluck(:id, :name)
    render json: @personality_masters
  end
end

