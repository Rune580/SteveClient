#version 460 core

#define AMBIENT_LIGHT (0.4)

out vec4 FragColor;

layout(location = 0) in vec2 texCoord;
layout(location = 1) flat in int atlas;

uniform vec4 tint;
uniform sampler2DArray textureSampler;
uniform sampler2DArray normalSampler;

uniform int[900] normalMap;

vec3 getLightDir() {
    vec3 light = vec3(10, -10, 10);
    
    return normalize(light);
}

vec3 getNormal() {
    vec3 sampledNormal = vec3(1, 1, 1);
    
    int normalLayer = normalMap[atlas];
    if (normalLayer > -1)
        sampledNormal = texture(normalSampler, vec3(texCoord, normalLayer)).rgb;
    
    return normalize(sampledNormal * 2.0 - 1.0);
}

vec3 getLightColor() {
    return vec3(1, 1, 1);
}

vec3 getAmbient() {
    vec3 lightCol = getLightColor();
    
    return lightCol * AMBIENT_LIGHT;
}

vec3 getDiffuse() {
    vec3 lightDir = getLightDir();
    vec3 normal = getNormal();
    float diff = max(dot(normal, lightDir), 0.0);

    return diff * getLightColor();
}

void main() {
    vec4 color = texture(textureSampler, vec3(texCoord, atlas)) * vec4(tint.rgb, 1);
    if (color.a == 0)
        discard;

    vec3 ambient = getAmbient();
    vec3 diffuse = getDiffuse();
    
    color.rgb = (ambient + diffuse) * color.rgb;
    
    FragColor = color;
}
