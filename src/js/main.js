/*
    Andor Saga
*/

// Cel Fragment Shader

let celShaderFrag = `
#ifdef GL_ES
  precision mediump float;
#endif

varying vec3 var_vertPos;
varying vec4 var_vertCol;
varying vec3 var_vertNormal;
varying vec2 var_vertTexCoord;

uniform sampler2D texture0;
uniform sampler2D texture1;
uniform vec3 mouse;
uniform vec2 res;
uniform float numShades;
uniform float time;

float aspect = res.x/res.y;

vec4 sample(vec2 offset){
  vec2 p = vec2(gl_FragCoord.xy + offset) / res;
  p.y = 1.0 - p.y;
  return texture2D(texture0, p);
}

void main() {
  vec2 p = (gl_FragCoord.xy / res);
  p.y = 1.0 - p.y;

  vec4 diffuse = texture2D(texture0, p);
  float intensity = (diffuse.r + diffuse.g + diffuse.b) / 3.0;
  vec4 result = vec4(  vec3(floor(intensity * numShades)/ numShades), 1.0);
  gl_FragColor = result;
  
  // // 0 -25%  Original
  // if( gl_FragCoord.x < res.x * 0.25){
  //    gl_FragColor = diffuse;
  // }
  // // 25% - 50%
  // else if(gl_FragCoord.x > 0.25 * res.x && gl_FragCoord.x < 0.5 * res.x){
  //   gl_FragColor = result;
  // }
  // // 50% - 75%
  // else if(gl_FragCoord.x > 0.5 * res.x && gl_FragCoord.x < 0.75 * res.x){  
  //   gl_FragColor = diffuse;
  // }
  // // 75% - 100%
  // else if(gl_FragCoord.x > 0.75 * res.x){
  //   gl_FragColor = result;
  // }

  gl_FragColor = vec4(gl_FragCoord.xy/res, 0, 1);
}`;

let celShaderVert = `
#ifdef GL_ES
  precision highp float;
  precision mediump int;
#endif

attribute vec3 aPosition;
attribute vec4 aVertexColor;
attribute vec3 aNormal;
attribute vec2 aTexCoord;

varying vec3 var_vertPos;
varying vec4 var_vertCol;
varying vec3 var_vertNormal;
varying vec2 var_vertTexCoord;

uniform mat4 uModelViewMatrix;
uniform mat4 uProjectionMatrix;
uniform mat3 uNormalMatrix;

void main() {
  gl_Position = uProjectionMatrix * uModelViewMatrix * vec4(aPosition, 1.0 );
  var_vertPos = aPosition;
  var_vertCol = aVertexColor;
  var_vertNormal = aNormal;
  var_vertTexCoord = aTexCoord;
}`;

let sobelShaderFrag = `
#ifdef GL_ES
  precision mediump float;
#endif

varying vec3 var_vertPos;
varying vec4 var_vertCol;
varying vec3 var_vertNormal;
varying vec2 var_vertTexCoord;

uniform sampler2D t1;

uniform vec3 mouse;
uniform vec2 res;
uniform float time;
uniform vec3 col;
uniform vec3 col2;

// vec2 sampling offsets
uniform float _[18];

float aspect = res.x/res.y;

mat3 sobel = mat3(  -1.0, 0.0, 1.0,
                    -2.0, .010, 2.0,
                    -1.0, 0.0, 1.0);

vec4 sample(vec2 offset){
  vec2 p = vec2(gl_FragCoord.xy + offset) / res;
  p.y = 1.0 - p.y;
  return texture2D(t1, p);
}

void main() {
  vec3 col = sample(vec2(0.)).rgb;
  // col.b = 1.-col.b;

  col.r = step(gl_FragCoord.x, gl_FragCoord.y);
  gl_FragColor = vec4(col,1);
}`;

let sobelShaderVert = `
#ifdef GL_ES
  precision highp float;
  precision mediump int;
#endif

attribute vec3 aPosition;
attribute vec4 aVertexColor;
attribute vec3 aNormal;
attribute vec2 aTexCoord;

varying vec3 var_vertPos;
varying vec4 var_vertCol;
varying vec3 var_vertNormal;
varying vec2 var_vertTexCoord;

uniform mat4 uModelViewMatrix;
uniform mat4 uProjectionMatrix;
uniform mat3 uNormalMatrix;

void main() {
  gl_Position = uProjectionMatrix * uModelViewMatrix * vec4(aPosition, 1.0 );
  var_vertPos = aPosition;
  var_vertCol = aVertexColor;
  var_vertNormal = aNormal;
  var_vertTexCoord = aTexCoord;
}`;

'use strict';

let YesMakeGif = false;
let gif;

Number.prototype.clamp = function(min, max) {
  return Math.min(Math.max(this, min), max);
};

function animate(time) {
  requestAnimationFrame(animate);
  // TWEEN.update(time);
}
requestAnimationFrame(animate);


/*
  fs - array of frag shaders
*/
function makeSketch(fs, params) {
  const DefaultSketchWidth = 320;
  const DefaultSketchHeight = 240;
  
  let mainShader;
  let w, h;
  let img0, img1, img2;

  let timeVal = { t: 0 };
  let sketchTime;
  let tracking = [];
  let easing = 0.05;
  let start = 0;
  let mouseIsDown = 0;
  let lastMouseDown= [120,200];

  let ctx1;
  let shader1;
  let testctx;
  let gfx3D;

  var sketch = function(p) {

    let renderers = {};
    let graphicsCtx = [];
    let progObjects = [];

    p.preload = function() {
      // same vertex shader for each frag shader
      let globalVs = `precision highp float;
                      varying vec2 vPos;
                      attribute vec3 aPosition;
                      void main() {
                        vPos = (gl_Position = vec4(aPosition,1.0)).xy;
                      }`;

      let mainFs = `precision mediump float;
                    uniform sampler2D lastBuffer;
                    void main(){
                      vec2 p = gl_FragCoord.xy/vec2(300.);
                      vec4 col = texture2D(lastBuffer, vec2(0.5));
                      gl_FragColor = vec4(col.rg,1, 1);
                    }`;

      let fs1 =    `precision mediump float;
                    void main(){
                      vec2 p = gl_FragCoord.xy/vec2(300.);
                      gl_FragColor = vec4(1, 1, 0, 1);
                    }`;
      
      mainShader = p.createShader(globalVs, mainFs);
      //new p5.Shader(p._renderer, globalVs, mainFs);

      ctx1 = p.createGraphics(w, h, p.WEBGL);

      gfx3D = p.createGraphics(w,h, p.WEBGL);

      shader1  = new p5.Shader(gfx3D._renderer, globalVs, fs1);

      // fs.forEach( _fs => {
      //   console.log('creating renderer', _fs);
      //   let ctx = p.createGraphics(w, h, p.WEBGL);
      //   graphicsCtx.push(ctx);
        
      //   let newShader = new p5.Shader(ctx._renderer, globalVs, _fs);
      //   progObjects.push(newShader);

      //   // progObjects.push(p.createShader(ctx, globalVs, _fs));
      // });

      // TODO: fix
      // if (params.tex0) {img0 = p.loadImage(params.tex0);}
      // if (params.tex1) {img1 = p.loadImage(params.tex1);}
      // if (params.tex2) {img2 = p.loadImage(params.tex2);}
    };

    p.setup = function() {
      w = params.width || DefaultSketchWidth;
      h = params.height || DefaultSketchHeight;
      sketchTime = 0;
      tracking = [0, 0];


      p.createCanvas(p.windowWidth, p.windowHeight, p.WEBGL);
      gfx = p.createGraphics(p.windowWidth, p.windowHeight);
      gfx3D = p.createGraphics(p.windowWidth, p.windowHeight, p.WEBGL);

      sobelShader = new p5.Shader(p._renderer, sobelShaderVert, sobelShaderFrag);
      celShader = new p5.Shader(gfx3D._renderer, celShaderVert, celShaderFrag);




      // var c = p.createCanvas(w, h, p.WEBGL);
      p.pixelDensity(1);

      // reset anim
      // c.mouseClicked(e=>{  start = p.millis();   });
      // c.mousePressed(e=>{  mouseIsDown = 1;      });
      // c.mouseReleased(e=>{
      //   mouseIsDown = 0;

      //   let x = p.mouseX.clamp(0, w);
      //   let y = p.mouseY.clamp(0, h);
      //   lastMouseDown = [x,y];
      // });

      $(p.canvas).appendTo($('#target'));
      p.loop();
    };


    /**
          Draw
    */
    p.draw = function() {
      let width = p.windowWidth;
      let height = p.windowHeight;

      gfx.push();
      gfx.translate(width / 2, height / 2);
      // gfx.imageMode(p.CENTER);
      // gfx.fill(255,0,0);
      // gfx.strokeWeight(3);
      // gfx.rect(0,0,130,130);
      gfx.pop();



      // CEL
      gfx3D.push();  
      gfx3D.translate(-width / 2, -height / 2);
      gfx3D.shader(celShader);
      celShader.setUniform('res', [width, height]);
      // celShader.setUniform('mouse', [pmouseX, height - pmouseY, mouse[0], mouse[2]]);
      celShader.setUniform('texture0', gfx);
      gfx3D.rect(0, 0, p.windowWidth, p.windowHeight, 1, 1);
      gfx3D.pop();
      


      // SOBEL
      p.push();
      p.translate(-width / 2, -height / 2);
      p.shader(sobelShader);
      sobelShader.setUniform('time', p.millis());
      sobelShader.setUniform('res', [width, height]);
      sobelShader.setUniform('_', [-1, -1, 0, -1, 1, -1, -1, 0, 0, 0, 1, 0, -1, 1, 0, 1, 1, 1]);
      // sobelShader.setUniform('mouse', [pmouseX, height - pmouseY, mouse[0], mouse[2]]);
      sobelShader.setUniform('t1', gfx3D);
      p.rect(0, 0, p.windowWidth, p.windowHeight, 1, 1);
      p.pop();


    };//end draw
  };
  return sketch;
}

let demo = {
  '0': {
    src: '../fragments/code/0xff/90/99/0.fs'
  },
  '1': {
    src: '../fragments/code/0xff/90/99/1.fs'
  }
};


function getFs0(){
  return fetch('../fragments/code/0xff/90/99/0.fs')
    .then(res => res.text())
    .then(fragShaderCode => {
      return fragShaderCode;
    });
}
function getFs1(){
  return fetch('../fragments/code/0xff/90/99/1.fs')
    .then(res => res.text())
    .then(fragShaderCode => {
      return fragShaderCode;
    });
}



(function load(){
  Promise.all([getFs0(), getFs1()])
    .then(fragShaders => {
      let relPath;
      let sketch = new p5(makeSketch(fragShaders,{}), relPath);
    });

  //   .then(res => res.text())
  //     .then(fragShaderCode => {
  //       let relPath = '';
  //       let fragShaders = [fragShaderCode]
  //       let sketch = new p5(makeSketch(fragShaders,{}), relPath);
  //     });

})();