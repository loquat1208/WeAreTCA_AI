Rails.application.routes.draw do
  #サイト関連
  get 'main/index'

  resources :enemies
  resources :players

  root 'main#index'

  #API関連
  namespace :api do
    resources :enemies, only: [:index] do
      collection do
        get :info
      end
    end

    resources :players, only: [:index] do
      collection do
        get :info
      end
    end

  end
end
