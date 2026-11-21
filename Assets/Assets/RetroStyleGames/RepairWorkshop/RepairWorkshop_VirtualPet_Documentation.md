# Repair Workshop – Retro Virtual Pet – Documentation

## Overview
Retro Virtual Pet from the Repair Workshop collection: an egg-shaped virtual pet key-chain toy with display, buttons, circuit board, speaker, battery, hatch, keyring and screws. It ships as one assembled prefab and as 19 separate part prefabs, so it can be shown whole, taken apart or used piece by piece. This pack is the VirtualPet part of the Repair Workshop complete pack: asset names, folders and GUIDs are identical, so it can be imported next to the complete pack or other Repair Workshop gadget packs without duplicates.

---

## Render Pipeline Compatibility
This asset is compatible with:

- Built-in Render Pipeline (BRP)
- Universal Render Pipeline (URP)
- High Definition Render Pipeline (HDRP)

All materials use the custom **RSG Shader Graphs** in RetroStyleGames/Base/Shaders (RSG_Lit) that support all three render pipelines, so a single material set works everywhere — no material swapping required. Authored with Unity 6000.0.77f1 and URP/HDRP/Shader Graph 17.0.4. Built-in projects need the Shader Graph package (URP and HDRP include it).

---

## Technical Details
| Prefab | Vertices | Triangles | Material slots | Collider | Size (m) |
|---|---|---|---|---|---|
| P_RSG_VirtualPet_Assembled | 26,470 | 28,548 | 19 | 19 Box (one per part) | 0.043 × 0.022 × 0.065 |
| P_RSG_VirtualPet_Battery | 262 | 228 | 1 | Box | 0.018 × 0.003 × 0.018 |
| P_RSG_VirtualPet_Buttons | 2,722 | 2,732 | 1 | Box | 0.024 × 0.007 × 0.007 |
| P_RSG_VirtualPet_Display | 1,973 | 2,024 | 1 | Box | 0.022 × 0.005 × 0.021 |
| P_RSG_VirtualPet_Front | 5,341 | 6,232 | 1 | Box | 0.043 × 0.010 × 0.051 |
| P_RSG_VirtualPet_Hatch | 1,959 | 2,768 | 1 | Box | 0.023 × 0.007 × 0.023 |
| P_RSG_VirtualPet_Holder | 616 | 558 | 1 | Box | 0.022 × 0.003 × 0.020 |
| P_RSG_VirtualPet_Keyring | 1,138 | 1,280 | 1 | Box | 0.001 × 0.013 × 0.013 |
| P_RSG_VirtualPet_PCB | 1,162 | 952 | 1 | Box | 0.025 × 0.003 × 0.032 |
| P_RSG_VirtualPet_Rear | 8,985 | 9,626 | 1 | Box | 0.043 × 0.012 × 0.056 |
| P_RSG_VirtualPet_Screw_01 | 222 | 208 | 1 | Box | 0.002 × 0.005 × 0.002 |
| P_RSG_VirtualPet_Screw_02 | 226 | 212 | 1 | Box | 0.003 × 0.010 × 0.003 |
| P_RSG_VirtualPet_Screw_03 | 330 | 308 | 1 | Box | 0.003 × 0.007 × 0.003 |
| P_RSG_VirtualPet_Screw_04 | 222 | 208 | 1 | Box | 0.002 × 0.005 × 0.002 |
| P_RSG_VirtualPet_Screw_05 | 222 | 208 | 1 | Box | 0.002 × 0.005 × 0.002 |
| P_RSG_VirtualPet_Screw_06 | 226 | 212 | 1 | Box | 0.003 × 0.010 × 0.003 |
| P_RSG_VirtualPet_Screw_07 | 222 | 208 | 1 | Box | 0.002 × 0.005 × 0.002 |
| P_RSG_VirtualPet_Screw_08 | 226 | 212 | 1 | Box | 0.003 × 0.010 × 0.003 |
| P_RSG_VirtualPet_Screw_09 | 226 | 212 | 1 | Box | 0.003 × 0.010 × 0.003 |
| P_RSG_VirtualPet_Speaker | 190 | 160 | 1 | Box | 0.010 × 0.003 × 0.010 |

- **Assembled gadget:** 26,470 vertices, 28,548 triangles (the 19 parts together; the assembled prefab nests the part prefabs).
- **Models:** 19 FBX (SM_RSG_VirtualPet_*). **Prefabs:** 20 (19 parts + 1 assembled).
- **Materials:** 1 (M_RSG_VirtualPet), plus 1 demo ground material. **Textures:** 3 (3 × 4096×4096).
- **Texture sets:** Base Color (_BC, sRGB), Metallic/AO/Smoothness (_MAS: R metallic, G AO, A smoothness, linear), Normal (_N, OpenGL).
- **Colliders:** one BoxCollider per part prefab. **UV:** UV0 for textures, UV1 for lightmaps on every mesh.
- **Rigged:** No. **Animations:** No. **LOD:** No. **Units:** metres, real-world size.

---

## Included Content
- P_RSG_VirtualPet_Assembled – the complete virtual pet toy.
- 19 part prefabs – P_RSG_VirtualPet_Battery, P_RSG_VirtualPet_Buttons, P_RSG_VirtualPet_Display, P_RSG_VirtualPet_Front, P_RSG_VirtualPet_Hatch, P_RSG_VirtualPet_Holder, P_RSG_VirtualPet_Keyring, P_RSG_VirtualPet_PCB, P_RSG_VirtualPet_Rear, P_RSG_VirtualPet_Screw_01, P_RSG_VirtualPet_Screw_02, P_RSG_VirtualPet_Screw_03, P_RSG_VirtualPet_Screw_04, P_RSG_VirtualPet_Screw_05, P_RSG_VirtualPet_Screw_06, P_RSG_VirtualPet_Screw_07, P_RSG_VirtualPet_Screw_08, P_RSG_VirtualPet_Screw_09, P_RSG_VirtualPet_Speaker.
- 1 material, 3 textures, 1 shared Shader Graph.
- 6 demo scenes (Demo/Scenes): RepairWorkshop_VirtualPet_Overview and RepairWorkshop_VirtualPet_Demo for BRP, URP and HDRP, with the demo ground material (Demo/Materials) and volume profiles (Demo/Profiles).

---

## Folder Structure
- RetroStyleGames/Base/Shaders – shared RSG Shader Graphs.
- RetroStyleGames/RepairWorkshop/Gadgets/VirtualPet/{Models, Materials, Prefabs, Textures} – the gadget.
- RetroStyleGames/RepairWorkshop/Demo – demo scenes, volume profiles and the ground material.
- RetroStyleGames/RepairWorkshop/RepairWorkshop_VirtualPet_Documentation.md – this documentation.
The Demo folder can be deleted without affecting the assets.

---

## Demo Scenes
Open the scenes that match your project's render pipeline (Demo/Scenes):

- **RepairWorkshop_VirtualPet_Overview_BRP / _URP / _HDRP** – a content overview: the assembled gadget followed by every part prefab, laid out in rows and lit by three directional lights.
- **RepairWorkshop_VirtualPet_Demo_BRP / _URP / _HDRP** – a teardown presentation: the assembled gadget next to all of its parts laid out flat on a studio ground, same three-light setup.

The scenes contain only prefab instances, a ground plane (Demo), lights, a camera and (URP / HDRP) a global volume. The package contains no scripts.

---

## Setup Instructions
0. Built-in Render Pipeline only: install Shader Graph (Window > Package Manager > Shader Graph, 17.0.4 or newer for Unity 6).
1. Import the package into a project that uses BRP, URP or HDRP. Use Linear color space (default for new Unity projects; required by HDRP).
2. Optional: open the demo scenes for your pipeline.
3. Drag P_RSG_VirtualPet_Assembled or any part prefab into your scene.

---

## Lighting Notes
- Every scene uses three realtime directional lights (key, rim, fill); only the key light casts soft shadows. No baked lighting data is included and no skybox is used.
- URP: the key light uses its own shadow bias (Depth 1, Normal 1) instead of the pipeline settings, which keeps the small parts free of shadow acne. A global volume adds tonemapping.
- HDRP: lights in lux (key 2,500, rim 1,000, fill 650); the global volume sets a fixed exposure and tonemapping.
- BRP: shadow bias 0.05 / normal bias 0.4; no post-processing.

---

## Notes
- Parts keep their pivot at the part origin; the assembled prefab places them with position offsets only.
- The full workshop environment with all six gadgets is available as the Repair Workshop complete pack.

---

## Support
If you have any questions or need assistance, please contact support via the Unity Asset Store page.

---

## License
This asset is provided under the standard Unity Asset Store license agreement.
