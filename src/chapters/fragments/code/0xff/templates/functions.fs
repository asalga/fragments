vec2 skew(vec2 p, float x){
  p.x+= p.y*x;
  return p;
}

float every(float v, float i, float c){
  return mod(v, i) * step(mod(v, c), i);
}
