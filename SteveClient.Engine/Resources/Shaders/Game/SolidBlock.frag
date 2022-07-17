#version 460 core

#define AMBIENT_LIGHT (0.4)

out vec4 FragColor;

layout(location = 0) flat in int atlas;
layout(location = 1) in vec2 texCoords;
layout(location = 2) in vec3 Normal;
layout(location = 3) in vec3 FragPos;
layout(location = 4) in vec3 tangentLightDir;
layout(location = 5) in vec3 tangentViewPos;
layout(location = 6) in vec3 tangentFragPos;

uniform vec4 tint;
uniform sampler2DArray textureSampler;
uniform sampler2DArray normalSampler;
uniform float displacementScale;
uniform vec3 viewPos;

uniform int[900] normalMap;

vec3 getLightDir() {
    return normalize(tangentLightDir);
}

vec2 parallaxMapping(vec2 uv, vec3 viewDir) {
    float height = 0;
    
    int normalLayer = normalMap[atlas];
    if (normalLayer > -1)
        height = texture(normalSampler, vec3(uv, normalLayer)).a;
    
    vec2 p = viewDir.xy / viewDir.z * (height * displacementScale);
    return uv - p;
}

vec3 getNormal(vec2 uv) {
    return Normal;
    
    vec3 sampledNormal = vec3(1);
    
    int normalLayer = normalMap[atlas];
    if (normalLayer > -1)
        sampledNormal = texture(normalSampler, vec3(uv, normalLayer)).rgb;
    
    return normalize(sampledNormal * 2.0 - 1.0);
}

vec3 getLightColor() {
    return vec3(1, 1, 1);
}

vec3 getAmbient() {
    vec3 lightCol = getLightColor();
    
    return lightCol * AMBIENT_LIGHT;
}

vec3 getDiffuse(vec2 uv) {
    vec3 lightDir = getLightDir();
    vec3 normal = getNormal(uv);
    
    float diff = max(dot(lightDir, normal), 0.0);

    return diff * getLightColor();
}

vec3 getSpec() {
    vec3 normal = getNormal(texCoords);
    vec3 lightDir = getLightDir();
    
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, normal);
    
    float spec = 0.0;
    
    vec3 halfDir = normalize(lightDir + viewDir);
    spec = pow(max(dot(normal, halfDir), 0.0), 32.0);
    
    return vec3(0.3) * spec; 
}

void main() {
    //vec3 viewDir = normalize(tangentViewPos - tangentFragPos);
    //vec2 uv = parallaxMapping(texCoords, viewDir);

    vec2 uv = texCoords;
    
    if(uv.x > 1.0 || uv.y > 1.0 || uv.x < 0.0 || uv.y < 0.0)
        discard;
    
    vec4 color = texture(textureSampler, vec3(uv, atlas)) * vec4(tint.rgb, 1);
    if (color.a == 0)
        discard;

    vec3 ambient = getAmbient();
    vec3 diffuse = getDiffuse(uv);
    vec3 specular = getSpec();
    
    color.rgb = (ambient + diffuse + specular) * color.rgb;
    
    FragColor = color;
}
