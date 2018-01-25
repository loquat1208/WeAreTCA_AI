class EnemiesController < ApplicationController
  before_action :set_enemy, only: [:show, :edit, :update, :destroy]
 
  # GET /enemies
  # GET /enemies.json
  def index
    @enemies = Enemy.all
  end

  # GET /enemies/1
  # GET /enemies/1.json
  def show
  end

  # GET /enemies/new
  def new
    @enemy = Enemy.new
  end

  # GET /enemies/1/edit
  def edit
  end

  # POST /enemies
  # POST /enemies.json
  def create
    @enemy = Enemy.new(enemy_params)
    respond_to do |format|
      if ( enemy_params[:hp].to_i + enemy_params[:mp].to_i + enemy_params[:speed].to_i + enemy_params[:power].to_i ) <= 100
        @enemy.save(enemy_params)
        format.html { redirect_to @enemy, notice: 'Enemy was successfully created.' }
        format.json { render :show, status: :created, location: @enemy }
      else
          format.html { redirect_to @enemy, alert: 'hp,mp,power,speedの合計が100以下になるようにしてください' }
          format.json { render :show, status: :ok, location: @enmey }
          logger.debug(enemy_params[:hp])
          logger.debug(enemy_params[:mp])
          logger.debug(enemy_params[:power])
          logger.debug(enemy_params[:speed])
          logger.debug(( enemy_params[:hp].to_i + enemy_params[:mp].to_i + enemy_params[:speed].to_i + enemy_params[:power].to_i ))
        end
    end
  end

  # PATCH/PUT /enemies/1
  # PATCH/PUT /enemies/1.json
  def update
    respond_to do |format|
        if ( enemy_params[:hp].to_i + enemy_params[:mp].to_i + enemy_params[:speed].to_i + enemy_params[:power].to_i ) <= 100
          @enemy.update(enemy_params)
          format.html { redirect_to @enemy, notice: 'Enemy was successfully updated.' }
          format.json { render :show, status: :ok, location: @enmey }
        else
          format.html { redirect_to @enemy, alert: 'hp,mp,power,speedの合計が100以下になるようにしてください' }
          format.json { render :show, status: :ok, location: @enmey }
          logger.debug(enemy_params[:hp])
          logger.debug(enemy_params[:mp])
 	  logger.debug(enemy_params[:power])
 	  logger.debug(enemy_params[:speed])
	  logger.debug(( enemy_params[:hp].to_i + enemy_params[:mp].to_i + enemy_params[:speed].to_i + enemy_params[:power].to_i ))
        end
    end
  end

  # DELETE /enemies/1
  # DELETE /enemies/1.json
  def destroy
    @enemy.destroy
    respond_to do |format|
      format.html { redirect_to enemies_url, notice: 'Enemy was successfully destroyed.' }
      format.json { head :no_content }
    end
  end

  def enemy_params
    params.require(:enemy).permit(:power, :speed, :hp, :skill, :mp, actions_attributes: [:id, :execution, :character, :parameter, :value, :comparison, :action, :_destroy])
  end

  private
    def save_enemy
      @enemy.save(enemy_params)
    end
    
    def update_enemy
      @enemy.update(enemy_params)
    end

    # Use callbacks to share common setup or constraints between actions.
    def set_enemy
      @enemy = Enemy.find(params[:id])
    end
end
