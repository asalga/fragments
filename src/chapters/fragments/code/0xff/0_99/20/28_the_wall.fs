precision mediump float;
uniform vec2 u_res;
uniform float u_time;
uniform vec3 u_mouse;
uniform sampler2D u_texture0;
uniform sampler2D u_texture1;


void main(){
  // play area :)
  float t = u_time*.1;
  float zScale = .8;
  float texScale = 2.;

  // color stuff
  const float shininess = 16.;
  const float lightPower = .8;
  vec3 ambientColor = vec3(0.);
  vec3 diffuseColor = vec3(0.96, 1., 1.);
  vec3 specColor = vec3(.84, .78, .45);

  vec2 a = vec2(1., u_res.y/u_res.x);
  vec2 p = a*(gl_FragCoord.xy/u_res*2.-1.);
  p /= texScale;

  vec3 m = vec3(a*(u_mouse.xy/u_res*2.)-1., 1.);
  m /= texScale;
  
  vec3 lightToFrag = m - vec3(p.x, -p.y, -1.);
  float lightDist = length(lightToFrag);
  lightDist = lightDist*lightDist;

  lightToFrag = normalize(lightToFrag);

  vec2 uv = mod(vec2(p.x,-p.y), 1.);
  vec4 diffuseMap = texture2D(u_texture0,uv);
  vec4 normalMap = texture2D(u_texture1,uv);
  vec3 n = vec3(normalMap.xy*2.-1.,normalMap.z*zScale);
  n = normalize(n);
  
  float lambertian = dot(n, lightToFrag);

  // SPEC
  vec3 reflectDir = reflect(-lightToFrag, n);
  float specAngle = dot(reflectDir, vec3(0., 0., 1.));
  // float specAngle = dot(reflectDir, vec3(0., 0., -1.));
  float specular = pow(specAngle, shininess*5.0);
  
  vec3 ambCalc = ambientColor;
  vec3 diffCalc = vec3(diffuseMap) * lambertian * diffuseColor * (lightPower/lightDist);
  vec3 specCalc = vec3(diffuseMap) * specular * specColor * (lightPower/lightDist)/1.1;

  vec3 finalCol = ambCalc + diffCalc + specCalc;

  gl_FragColor = vec4(finalCol, 1.);
}