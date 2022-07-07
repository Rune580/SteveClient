#version 330 core

out vec4 FragColor;

in vec2 texCoord;

uniform vec4 color;
uniform sampler2D textureSampler;

void main() {
    float dist = 0.5 - texture(textureSampler, texCoord).r;
    
    vec2 dDist = vec2(dFdx(dist), dFdy(dist));
    
    float pixelDist = dist / length(dDist);
    
    float alpha = clamp(0.5 - pixelDist, 0, 1);
    if (alpha == 0)
        discard;
    
    vec4 text = vec4(1, 1, 1, alpha);
    FragColor = color * text;
}
