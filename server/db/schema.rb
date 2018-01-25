# This file is auto-generated from the current state of the database. Instead
# of editing this file, please use the migrations feature of Active Record to
# incrementally modify your database, and then regenerate this schema definition.
#
# Note that this schema.rb definition is the authoritative source for your
# database schema. If you need to create the application database on another
# system, you should be using db:schema:load, not running all the migrations
# from scratch. The latter is a flawed and unsustainable approach (the more migrations
# you'll amass, the slower it'll run and the greater likelihood for issues).
#
# It's strongly recommended that you check this file into your version control system.

ActiveRecord::Schema.define(version: 20180125013217) do

  create_table "actions", force: :cascade do |t|
    t.integer "execution"
    t.integer "character"
    t.integer "parameter"
    t.integer "value"
    t.integer "comparison"
    t.integer "action"
    t.integer "enemy_id"
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
    t.index ["enemy_id"], name: "index_actions_on_enemy_id"
  end

  create_table "enemies", force: :cascade do |t|
    t.integer "power"
    t.integer "speed"
    t.integer "hp"
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
    t.integer "skill"
    t.integer "mp"
  end

  create_table "enemy_actions", force: :cascade do |t|
    t.integer "execution"
    t.integer "character"
    t.integer "parameter"
    t.integer "value"
    t.integer "comparison"
    t.integer "action"
    t.integer "enemy_id"
    t.datetime "created_at", null: false
    t.datetime "updated_at", null: false
    t.index ["enemy_id"], name: "index_enemy_actions_on_enemy_id"
  end

end
