# Beyond the Door of Memory

> 한국어 설명은 [여기](#korean)를 참고하세요.

A 2D emotional narrative adventure game built in Unity —  
collect fragments of lost memory in a dreamlike subconscious world  
and uncover the hidden truth, with **multiple endings shaped by your choices**.

---

## Gameplay & Level Design

Each stage represents a shard of memory, designed with a unique psychological gimmick
to pressure the player emotionally and mechanically.

| Stage | Difficulty | Mechanic |
|---|---|---|
| Park of Memories | Easy | Tutorial — walking, interaction basics |
| The Day of the Argument | Medium | Broken mirror fragments trigger **camera shake** on collision — visualizing psychological trauma |
| The Accident Scene | Hard | Stealth — evade a pursuing **"Distorted Memory"** enemy. 3 hits = Game Over |
| Silence | Hard | Periodic **full-map blackouts** + random repositioning of memory fragment items |

---

## Key Systems

**Multiple Ending & Scene Control**
Player choices are tracked as state values. Dialogue UI drives branching scene transitions dynamically — the same choice node can route to different story paths depending on accumulated state.

**AI Pursuit System**
The "Distorted Memory" enemy detects the player's position within a defined range and pursues using a custom tracking logic. The player must use the environment to break line-of-detection.

**Dynamic Environment Algorithm**
The blackout event (Stage 4) runs a timer-controlled coroutine that triggers darkness, then uses `Random.Range` to reassign item `Transform` positions — creating a live difficulty spike without level reloads.

**Visual Feedback**
Camera shake script activates on obstacle collision, amplifying physical impact and tension through screen-space displacement.

---

## Endings

Player choices branch the story toward one of two endings.

### Bad Ending
https://github.com/lalunru/unity-2d-adventure-memory/blob/master/assets/%EA%B8%B0%EC%96%B5%EC%9D%98%EB%AC%B8%EB%84%88%EB%A8%B8_End_1.mp4

### True Ending
https://github.com/lalunru/unity-2d-adventure-memory/blob/master/assets/%EA%B8%B0%EC%96%B5%EC%9D%98%EB%AC%B8%EB%84%88%EB%A8%B8_End_2.mp4

---

## Tech Stack

| | |
|---|---|
| Engine | Unity 2D |
| Language | C# |
| Platform | PC (Windows) |

---

## License

This project was developed as a team academic project.  
Assets and story content are not licensed for redistribution.

---

<a name="korean"></a>
## 한국어 요약

무의식 속 꿈의 공간을 배경으로 흩어진 기억의 조각을 수집하며 숨겨진 진실에 다가가는 2D 감정 서사형 어드벤처 게임입니다. 플레이어의 선택에 따라 결말이 달라지는 다중 엔딩 시스템을 채택했습니다.

**핵심 구현**
- 선택지 기반 씬 전환 및 스토리 분기 시스템
- AI 추적 몬스터 (잠입 요소)
- 암전 + 아이템 랜덤 재배치 동적 환경 알고리즘
- 카메라 쉐이크 시각적 피드백
