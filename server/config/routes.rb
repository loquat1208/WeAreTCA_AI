Rails.application.routes.draw do
  resources :enemies
  root 'enemies#index'
  # For details on the DSL available within this file, see http://guides.rubyonrails.org/routing.html
  namespace :api do
    resources :enemies, only: [:index] do
      collection do
        get :actions
      end
    end
  end

end
