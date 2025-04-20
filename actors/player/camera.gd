extends Node3D


@onready var skin: Node3D = get_node("../Skin")
@export var mouse_sensitivity: float = 0.25 / 3
var camera_input_direction := Vector2.ZERO
# @export var spring_arm_lenght:float = 3.0
# @onready var spring_arm = get_node("SpringArm3D")


func _ready() -> void:
	Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
	# spring_arm.set("spring_length",spring_arm_lenght)


func _unhandled_input(event: InputEvent) -> void:
	var is_camera_motion := event is InputEventMouseMotion
	if is_camera_motion:
		camera_input_direction = event.screen_relative * mouse_sensitivity


func _physics_process(_delta: float) -> void:
	hide_skin_on_camera_too_close()
	rotation.x -= camera_input_direction.y * _delta
	rotation.y -= camera_input_direction.x * _delta
	rotation.x = clamp(rotation.x, deg_to_rad(-45), deg_to_rad(30))
	camera_input_direction = Vector2.ZERO

	# print (Engine.get_frames_per_second())


func hide_skin_on_camera_too_close() -> void:
	if get_node("SpringArm3D").get_hit_length() < 1.6:
		skin.visible = false
	else:
		skin.visible = true
