# Project: Line 0

> 한국어 설명은 [여기](#korean)를 참고하세요.

A first-person VR horror adventure game for **Meta Quest 2**,  
inspired by the Korean internet horror genre *"Neapolitan horror"* (나폴리탄 괴담).

You are the midnight manager of **Line 0** — an unmarked subway line that appears on no map.  
Follow the rules. Survive 3 nights.

---

## Gameplay

Each night introduces new, increasingly strange operating rules.  
Breaking a rule ends the game.

| Rule | Mechanic |
|---|---|
| **1 — Gaze restriction** | Looking directly at a corrupted advertisement board triggers visual distortion |
| **2 — Time attack** | Retrieve the track inspection log before an irregular train departs |
| **3 — NPC interaction** | Mirror the unknown station worker's gestures (90° bow, hand flip) with your own body |
| **4 — Event response** | Re-sync the system panel within the time limit when a distorted announcement plays |
| **5 — Puzzle (finale)** | 3 random rules applied simultaneously + use a UV flashlight to find a hidden code and reboot the central control system |

---

## Architecture

Systems were designed with extensibility in mind — adding a new rule requires no changes to existing code.

**`GameManager.cs`**
Central state controller. Manages current night, active rule states, level clear, and restart logic.

**`RuleEventBase.cs`**
Abstract base class inherited by all individual rules (Rule 1–5).
Standardizes `StartRule()`, `CompleteRule()`, and `FailRule()` — new rules are added by subclassing, not by modifying core logic.

---

## Optimization & Troubleshooting

**VR motion sickness (frame rate)**
Early builds failed to hit target FPS, causing motion sickness in playtesters.
Fixed by: removing all real-time lights → switching to 100% baked lightmaps, and downscaling texture resolutions.

**Physics interaction bug**
Grabbing items in VR produced erratic collision behavior and poor feel.
Fixed by: changing the interaction model from physics-based grab to touch-based activation, with added highlight feedback for clarity.

---

## Tech Stack

| | |
|---|---|
| Engine | Unity 3D (URP) |
| Platform | Meta Quest 2 |
| Framework | XR Interaction Toolkit |
| Language | C# |

---

## Gameplay Video

[Watch gameplay footage](./assets/Test_Video.mp4)

---

## License

This project was developed as a team academic project.  
Assets and content are not licensed for redistribution.

---

<a name="korean"></a>
## 한국어 요약

나폴리탄 괴담 장르를 기반으로 제작된 Meta Quest 2용 1인칭 VR 공포 어드벤처 게임입니다. 지도에도 없는 미지의 지하철 노선 '라인 0'의 심야 관리자가 되어, 매일 새롭게 추가되는 기이한 업무 수칙을 완수하며 3일간 생존해야 합니다.

**핵심 구현**
- `RuleEventBase.cs` 추상 클래스 기반 규칙 시스템 — 새 규칙 추가 시 기존 코드 수정 불필요
- `GameManager.cs` 중앙 상태 관리
- VR 멀미 개선: 실시간 조명 제거 → 100% Baked 라이트맵 전환
- 물리 상호작용 버그 수정: 터치 기반 인터랙션으로 전환 + 하이라이트 피드백 추가
