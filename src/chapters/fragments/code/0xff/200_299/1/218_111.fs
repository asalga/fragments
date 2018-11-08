// 218 - 111
//
precision mediump float;

uniform vec2 u_res;
uniform float u_time;

float sdRect(in vec2 p, in vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max(d, 0.));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}

float grid(in vec2 p){
  // vec2 p = gl_FragCoord.xy;
  vec2 lineWidthInPx = vec2(.01);
  vec2 cellSize = vec2(.25);
  vec2 i = step(mod(p, cellSize), lineWidthInPx);
  return i.x + i.y;
}


float easeInOutBack(float t, float b, float c, float d){
  float s = .70158;
  if ((t /= d/2.) < 1.){
    return c/2.*(t*t*(((s*=(1.525))+1.)*t - s)) + b;
  }
  return c/2.*((t-=2.)*t*(((s*=(1.525))+1.)*t + s) + 2.) + b;
}

float quint(float t){
  return pow(t, 5.);
}

void main(){
  vec2 p = gl_FragCoord.xy/u_res;
  float i;
  float sz = 1./4.;
  float t = u_time * .5;
  vec2 rectDims = vec2(sz/2.);

  vec2 bPos[4];
  float bricks[4];

  float s = -sz/2.;

  vec2 pgrid = p;
  if(mod(floor(t), 2.) == 0.){
    p.x = 1.-p.x;
  }

  bPos[0] = vec2(s         , -1. - sz/2.);
  bPos[1] = vec2(s - sz*1. , -1. - sz/2. - sz);
  bPos[2] = vec2(s - sz*2. , -1. - sz/2. - sz * 2.);
  bPos[3] = vec2(s - sz*3. , -1. - sz/2. - sz * 3.);

  float ft = fract(t);

  vec2 c = vec2(0., -1.0 - sz/2.);


  float speed = fract(t) * sz * 4.;

  // speed = easeInOutBack(speed*4., 0., 1., 1.);
  speed = quint(speed*1.) * 2.;

  vec2 fc0 = vec2(0., speed * 8.);
  fc0.y = clamp(fc0.y, 0., sz*2.);

  vec2 fc1 = vec2(0., speed * 2.5);
  fc1.y = clamp(fc1.y, 0., sz*3.);

  vec2 fc2 = vec2(0., speed * 3.);
  fc2.y = clamp(fc2.y, 0., sz*4.);

  vec2 fc3 = vec2(0., speed * 6.5);
  fc3.y = clamp(fc3.y, 0., sz*5.);

  vec2 start = vec2(0., 0.25);
  vec2 globalDisp = vec2(0., fract(t)*sz);

  float curr0 = step(sdRect(p + bPos[0] + fc0 + globalDisp, rectDims* 0.95), 0.);
  float curr1 = step(sdRect(p + bPos[1] + fc1 + globalDisp, rectDims* 0.95), 0.);
  float curr2 = step(sdRect(p + bPos[2] + fc2 + globalDisp, rectDims* 0.95), 0.);
  float curr3 = step(sdRect(p + bPos[3] + fc3 + globalDisp, rectDims* 0.95), 0.);

  // float curr1 = step(sdRect(p + bPos[1] + fc1, rectDims), 0.);
  // float curr2 = step(sdRect(p + bPos[2] + fc2, rectDims), 0.);
  // float curr3 = step(sdRect(p + bPos[3] + fc3, rectDims), 0.);

  // float curr = step(sdRect(p + c + fc, rectDims), 0.);
  float total = step(sdRect(p - start + globalDisp, vec2(1., sz*1.)), 0.);


  // i = curr0;
  // i = total + curr0 + curr1 + curr2;
  // i = clamp(i, 0., 1.);

  // i += total;
  pgrid.x += 0.01/2.;
  pgrid.y += t/4. + 0.005;

  i = total - grid(pgrid);
  i = clamp(i, 0., 1.);

  i += curr0 + curr1 + curr2 + curr3;

  gl_FragColor = vec4(vec3(i),1.);
}