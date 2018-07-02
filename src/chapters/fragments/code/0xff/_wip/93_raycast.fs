precision mediump float;

// resources
// andorsaga.wordpress.com/2014/01/22/understanding-raycasting-step-by-step/
// en.wikipedia.org/wiki/Digital_differential_analyzer_%28graphics_algorithm%29	
// andorsaga.files.wordpress.com/2014/01/raycast-distortion-fix.jpg

// Cast 'width' number of rays into the scene
// But since we're working with a frag shader
// we'll have some added benefits of working with Y
// so later on texture mapping will be straightforward

#define ZERO 0.
#define ONE 1.
#define PI 3.141592658
#define FOV 45.
#define MAX_DDA_STEPS 10.
#define TAU PI*2.
#define DEG_TO_RAD PI/180.
#define USE_SHADING 1.
#define USE_TEXTURES 0.
#define NORM_2(v) (v*2.-1.)

uniform vec2 u_res;
uniform sampler2D u_texture;


float background(vec2 p){
  return step(0.,p.y);
}

vec2 perp(in vec2 v){
  return vec2(-v.y,v.x);
}

void main(){
	vec2 a = vec2(1., u_res.y/u_res.x);
	vec2 p = a*(NORM_2(gl_FragCoord.xy/u_res));

	// --- Initial vars
	vec2 playerPos = vec2(ZERO);
	vec2 playerDir = vec2(ZERO,ONE);
	vec2 playerRight = perp(playerDir);


	// --- The FOV determines the angle the rays shoot into the scene
	// Calculate the base of the viewing triangle so that we know how far 
	// the rays span. 
	float camRatio = tan(FOV/2. * DEG_TO_RAD);


	// --- Calculate sweep ray
	// p is already normalized, so it saves us having to do it here.
	// Sweep across the viewport, scaling the right vector.
	// When currSweep=0, the ray is collinear with playerDir
	float currSweep = p.x;
	vec2 sweepRay = playerDir + (playerRight * camRatio * currSweep);


	// --- Find diagonal length between edges
	// What is the mag from one x edge to the next x edge?
	// The 'steeper' the sweepRay, the greater the value
	// magnitude of how much to 'step' x and y each tile
	// For x, note that the base length of the triangle
	// formed from one step to the other will have length = 1

	// Since our ray is normalized, the x will be less than 1 meaning
	// we can 'stretch' it so it reaches the next x-edge. Of course,
	// we'll need to do the same for the y to maintain the ray direction.
	vec2 magBetweenEdges = vec2(length(sweepRay * (1./sweepRay.x)),
								length(sweepRay * (1./sweepRay.y)));


	// --- Calculating Starting Mag/Offset
	// For the most part, the user will not be situated right on 
	// integer values. They will be at an 'offset'.
	// The diagonal length we calculated can be used, but we'll have
	// to give it a proper starting position.
	// 
	// If going right,
	//  - Put the position in the world (floor)
	//  - go the the 'next' cell (+1)
	//  - subtract player pos (-playerPos)
	//  - multiply by magBetweenEdges to get the percentage
	//  
	vec2 worldIndex = vec2(floor(playerPos));
	float magToEdgeX,
          magToEdgeY;
    float dirStepX, dirStepY;

	if(sweepRay.x > ONE){
		magToEdgeX = (worldIndex.x + ONE - playerPos.x) * magBetweenEdges.x;
		dirStepX = ONE;//
	}
	else {
		magToEdgeX = (playerPos.x - worldIndex.x) * magBetweenEdges.x;
		dirStepX = -ONE;//
	}

	if(sweepRay.y > ONE){
		magToEdgeY = (worldIndex.y + ONE - playerPos.y) * magBetweenEdges.y;
		dirStepY = ONE;//
	}
	else {
		magToEdgeY = (playerPos.y - worldIndex.y) * magBetweenEdges.y;
		dirStepY = -ONE;//
	}
	// TODO: look into zero!
	// dirStep = NORM_2(step(0., sweepRay))
	// vec2 dirStep = sign(sweepRay);


	// If ray.x > 0.0
	// (worldIndexX +1 - pos.x) * magBetweenEdges.x



	// -------
	// Run the DDA (Digital Differential Analyzer)

	// X and Y values 'race'. We compare each and increase the lesser value.
	// Loop terminates on reaching a non-empty cell.
	// We're in GLSL so we require a constant expression 
	// in the comparison part of our loop--just break out early.

	// Keep track if we hit the X side or Y side
	
	float sideHitX;
	for(float s = ZERO; s < MAX_DDA_STEPS; s += ONE){

		//
		if(magToEdgeX < magToEdgeY){
			sideHitX = ZERO;
		}
		else{
			sideHitX = ONE;
		}


		// sideHitX = step(magToEdgeX, magToEdgeY);

	}


	// --- Selecting color & light
	// We kept track of the non-empty index we hit, so we can use this to index 
	// into the map and get the associated color for the cell.	worldIndexX;
	worldIndexY;



	
	// darken the color to provide a better perspective if user hit X or Y side
	if(sideHitX == ONE){
		// wallColor = wallColor / 2.;
	}

	// --- Calculate line/slice height
	// The farther away the wall sliver is, the smaller it will be faking
	// foreshortening.
	// If the ray was 1 unit fromthe wall, the line would fill the entire
	// height of the viewport

	float hit = 0.;
	float lineHeight = 0.4;
	float wallDist = 0.;
	
	// if the fragment
	// if(p.y < lineHeight){
	if(lineHeight > abs(p.y)){
		hit = 1.;
	}


	// --- Distortion Correction
	// The farther the wall is from the player, the smaller the vertical 
	// slice needs to be on the screen. Keep in mind when viewing a 
	// wall/plane dead on, the length of the rays further out will be longer 
	// resulting in shorter ‘scanlines’. This isn’t desired since it will
	// warp our representation of the world.

	float i =   0.0 +
				hit + 
				// background(p) +
				0.0;

	gl_FragColor = vec4(vec3(i), 1.);
}