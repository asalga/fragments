// SDFs

float sdCircle(vec2 p, float r){
  return length(p)-r;
}

float sdRect(vec2 p, vec2 sz){
  vec2 d = abs(p)-sz;
  float _out = length(max( d , 0. ));
  float _in = min(max(d.y,d.x),0.);
  return _in + _out;
}




float sdSphere(vec3 p, float r){
	return length(p)-r;
}

float sdCylinder(vec3 p, vec2 sz){
  vec2 d = abs(vec2(length(p.xz),p.y)) - sz;
  float _out = length(max(vec2(d.x,d.y), 0.));
  float _in = min(max(d.x,d.y), 0.);
  return _in + _out;
}

float sdCone(vec3 p, vec3 c){
  vec2 d = vec2( length(p.xz), p.y );
  float d1 = -d.y-c.z;
  float d2 = max( dot(d,c.xy), d.y);
  return length(max(vec2(d1,d2),0.0)) + min(max(d1,d2), 0.);
}

float cubeSDF(vec3 p, vec3 sz) {
  vec3 d = abs(p) - sz
  float _in = min(max(d.x, max(d.y, d.z)), 0.);
  float _out = length(max(d, 0.));
  return _in + _out;
}

float sdSquarePyramid(){}
float sdTorus(){}
float sdGear(){}