#define SECTION_SIZE (16)

vec3 GetLocalBlockPos(vec3 vertPos, vec3 normal) {
    vec3 shiftedVert = vertPos + ((-normal) * 0.1);
    shiftedVert += normal * 10;
    
    float value = floor(min(min(shiftedVert.x, shiftedVert.y), shiftedVert.z));
    
    return vec3(value);
}

int GetIndexedBlockPos(vec3 vertPos, vec3 normal) {
    vec3 pos = GetLocalBlockPos(vertPos, normal);
    
    return int((((pos.y * SECTION_SIZE) + pos.z) * SECTION_SIZE) + pos.x);
}