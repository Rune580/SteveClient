#version 330 core

out vec4 FragColor;

in vec2 texCoord;

uniform vec4 color;
uniform sampler2D textureSampler;

void main() {
    vec4 sampled = vec4(1, 1, 1, texture(textureSampler, texCoord).r);
    
    if (sampled.a < 0.5)
        discard;
    
    FragColor = color * sampled;
}
