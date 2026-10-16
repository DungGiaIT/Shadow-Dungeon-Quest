# Shadow Dungeon — Bàn giao Week 2 cho agent của Dũng và Hoàng

Ngày đối chiếu: **16/10/2026**. Đây là tài liệu yêu cầu và hướng dẫn triển khai, chưa phải báo cáo xác nhận nghiệm thu.

## 1. Cách dùng tài liệu

Gửi cùng một file này cho cả hai người. Agent của Dũng dùng mục 1–5, 7–10 và prompt Dũng ở mục 11. Agent của Hoàng dùng mục 1–4, 6–10 và prompt Hoàng ở mục 11. Mỗi agent cần truy cập bản nguồn DemoDungeon cùng snapshot và bản project đích riêng để thực hiện phần việc của mình.

“Tách từ project” trong tài liệu này được hiểu là kế thừa các hệ thống cần thiết từ DemoDungeon sang bản Shadow Dungeon Week 2, chia quyền sửa theo người và tích hợp thành **một game chung**. Không tạo hai game độc lập. Đây là cách triển khai đề xuất từ yêu cầu của người dùng; milestone không quy định phải viết lại framework hoặc xóa hệ thống sample.

## 2. Nguồn yêu cầu và phạm vi Week 2

- [Milestone 2 — Week 2: Core Gameplay Prototype](https://github.com/DungGiaIT/Shadow-Dungeon/milestone/2), hạn **16/10/2026**.
- [Issue cha Hoàng #8](https://github.com/DungGiaIT/Shadow-Dungeon/issues/8): Phạm Huy Hoàng, `@Hoang271205`, 4 task, **16 giờ**.
- [Issue cha Dũng #9](https://github.com/DungGiaIT/Shadow-Dungeon/issues/9): Nguyễn Tiến Dũng, `@DungGiaIT`, 4 task, **16 giờ**.
- [Nguồn DemoDungeon](https://github.com/Hoang271205/DemoDungeon), đã đối chiếu mã nguồn local tại `/home/dunggiait/Developer/Unity/DemoDungeon`.
- [Repository đích Shadow-Dungeon](https://github.com/DungGiaIT/Shadow-Dungeon), đã đọc danh sách thư mục gốc qua GitHub; chưa xác minh toàn bộ mã nguồn đích giống bản local.

Đã đọc toàn bộ **10 issue** trong milestone: 2 issue cha và 8 issue con, gồm cả trạng thái mở/đóng. Tại thời điểm đọc, cả 10 đang mở và không có comment. Người phụ trách lấy theo **nội dung issue**: nhiều task con và issue cha Hoàng chưa có GitHub assignee; điều đó không thay đổi phân công ghi trong body. Không cộng thêm giờ cho issue cha: tổng **32 giờ**, mỗi task con đã gồm kiểm tra chéo.

Mục tiêu bắt buộc:

1. Prototype co-op cục bộ: hai phiên Direct IP cùng chơi, chọn nhân vật, tải scene và nhìn thấy nhau.
2. WASD, camera theo nhân vật, dungeon có đường từ spawn tới combat và boss.
3. Targeting, đánh thường và ít nhất một kỹ năng đặc trưng; HP thay đổi và có animation/hiệu ứng.
4. Công tắc sàn mở cửa; cửa đóng chặn đường, mở cho đi qua. Trình bày được Collider, Trigger, layer collision và NavMesh.
5. Enemy/boss có AI cơ bản, damage và life state; boss chết → thắng, cả đội gục → thua, hai phiên cùng kết quả.
6. Build chạy ngoài Editor; video hai góc nhìn, minh chứng ít nhất hai cơ chế, game loop, WBS và commit/PR của cả hai.

Các mã W2-R1, W2-R2, W2-R3, W2-Presentation bên dưới được giữ theo issue. Tài liệu không tự gán thêm định nghĩa cho các mã này. Không mở rộng sang nội dung Week 3/4. Online Sessions/Relay có sẵn được giữ nếu cần cho dependency, nhưng không phải mục tiêu nghiệm thu của tuần này.

## 3. Hiện trạng DemoDungeon đã đọc

### 3.1. Snapshot và môi trường

- HEAD local: `a96a17b4067bb092d433abb6629d058c24ade00d`.
- Branch local: `codex/upgrade-unity-packages-6000-5`.
- Remote local: `https://github.com/Hoang271205/DemoDungeon`.
- **Có nhiều thay đổi chưa commit** ở connection, test, scene, asset, package và ProjectSettings. HEAD trên không đại diện đầy đủ cho nội dung đang có trên ổ đĩa.
- `README.md` ghi Unity **6000.0.52f1**, nhưng `ProjectSettings/ProjectVersion.txt` local ghi **6000.5.8f1**. Agent Dũng phải chốt phiên bản thực sự mở/compile được và cập nhật hướng dẫn trước khi giao nền cho Hoàng; không dùng lẫn hai bộ package.
- `Packages/manifest.json` local: NGO `2.13.3`, Cinemachine `3.1.7`, Input System `1.20.0`, AI Navigation `2.0.15`, URP `17.5.0`, Multiplayer Services `2.3.2`, Authentication `3.7.4`, VContainer `1.18.0`.

Đây là kết quả đọc cấu hình và mã nguồn, **chưa chạy Unity, build hoặc kiểm thử hai phiên**. Không xem sự tồn tại của script là bằng chứng task đã hoàn thành.

### 3.2. Hệ thống có thể kế thừa

Các đường dẫn dưới đây tương đối với gốc DemoDungeon để dùng được trên máy của cả hai người.

| Hệ thống | Đường dẫn cần đọc/kế thừa | Hành vi thấy trong mã nguồn; việc còn phải xác minh |
| --- | --- | --- |
| Bootstrap và DI | `Assets/Scripts/ApplicationLifecycle/ApplicationController.cs`, `Assets/Scripts/Infrastructure`, các `.asmdef` | Bootstrap đăng ký VContainer, message channel, connection và dịch vụ; sau đó tải MainMenu. Không chỉ copy một scene gameplay rồi bỏ bootstrap. |
| Input WASD | `Assets/Scripts/Gameplay/UserInput/ClientInputSender.cs`, `Assets/InputSystem/PlayerActions.inputactions` | Input chỉ bật cho client owner; WASD theo hướng camera, chuẩn hóa hướng chéo, gửi đích NavMesh khoảng 0,04 giây/lần và gửi hủy khi nhả phím. Cần test dừng, focus UI và cả host/client. |
| Movement | `Assets/Scripts/Gameplay/GameplayObjects/Character/ServerCharacterMovement.cs`, `Assets/Scripts/Navigation` | Server dùng NavMeshAgent, DynamicNavPath và Rigidbody. Giữ mô hình server quyết định chuyển động. |
| Direct IP | `Assets/Scripts/ConnectionManagement`, `Assets/Scripts/Gameplay/UI/IPHostingUI.cs`, `IPJoiningUI.cs`, `IPUIMediator.cs` | Luồng IP dùng UnityTransport.SetConnectionData. Mặc định thực tế là **127.0.0.1:9998**. GetPlayerId có fallback GUID/profile khi chưa đăng nhập UGS; vẫn phải test luồng UI/bootstrap khi offline. |
| Chọn nhân vật | `Assets/Scripts/Gameplay/GameState/ServerCharSelectState.cs`, `ClientCharSelectState.cs`, `NetworkCharSelection.cs` | Có trạng thái chọn/khóa nhân vật và server tải BossRoom khi sẵn sàng. Cần xác minh hai người chọn được và spawn đúng. |
| Camera | `Assets/Scripts/CameraUtils/CameraController.cs`, `Assets/Scripts/Gameplay/GameplayObjects/Character/ClientCharacter.cs`, `Assets/Prefabs/GameCam` | ClientCharacter thêm CameraController cho owner; controller tìm tag CMCamera, đặt Follow/LookAt, dùng CinemachineCamera và CinemachineOrbitalFollow. Không áp dụng API Cinemachine 2 vào bản này. |
| Combat | `Assets/Scripts/Gameplay/Action`, `Assets/Scripts/Gameplay/Configuration/CharacterClass.cs`, `Assets/GameData/Action`, `Assets/GameData/Character` | Có action request/RPC, đánh gần, projectile, kỹ năng và animation. MeleeAction gọi ReceiveHitPoints với giá trị âm để gây damage. Cần xác minh asset skill, target, cooldown và đồng bộ phản hồi. |
| HP/life/AI | `Assets/Scripts/Gameplay/GameplayObjects/Character/ServerCharacter.cs`, thư mục `Character/AI`, `DamageReceiver.cs`, `NetworkHealthState.cs`, `NetworkLifeState.cs` | Server nhận damage, cập nhật HP/life; AI có Idle/Attack. HP âm là damage, dương là heal; không đảo quy ước khi nối combat. |
| Công tắc/cửa | `Assets/Scripts/Gameplay/GameplayObjects/FloorSwitch.cs`, `SwitchedDoor.cs`, `Assets/Prefabs/Game/InteractiveFloorSwitch.prefab`, `InteractiveBossDoor.prefab` | FloorSwitch đếm collider trên server và đồng bộ IsSwitchedOn. Door mở khi **ít nhất một** switch được liên kết đang bật, rời hết thì đóng. Không mặc định đổi thành cần hai người đứng đồng thời. |
| Thắng/thua | `Assets/Scripts/Gameplay/GameState/ServerBossRoomState.cs`, `ServerPostGameState.cs`, `NetworkPostGame.cs`, `PersistentGameState.cs` | Có nhánh ImpBoss Dead → Win, mọi player không còn Alive → Loss; tải PostGame qua network scene manager. Có delay thắng 7 giây/thua 2,5 giây. Cần test hai nhánh và nguy cơ gọi kết thúc nhiều lần. |
| Scene và NavMesh | `Assets/Scenes`, `Assets/Prefabs/Dungeon`, `Assets/Scenes/BossRoom/NavMesh-DungeonNavMesh.asset` | Có BossRoom và ba dungeon scene additive. Layout thay đổi phải đối chiếu NavMesh, spawn, camera, network object và loader. |

### 3.3. Scene bắt buộc đối chiếu khi tách

`ProjectSettings/EditorBuildSettings.asset` local đang bật đủ:

```text
Assets/Scenes/Startup.unity
Assets/Scenes/MainMenu.unity
Assets/Scenes/CharSelect.unity
Assets/Scenes/BossRoom.unity
Assets/Scenes/PostGame.unity
Assets/Scenes/BossRoom/DungeonBossRoom.unity
Assets/Scenes/BossRoom/DungeonEntrance.unity
Assets/Scenes/BossRoom/DungeonTransition.unity
```

Luồng chính: `Startup → MainMenu → Direct IP → CharSelect → BossRoom → PostGame`.

BossRoom có tham chiếu ba tên scene dungeon tương ứng qua loader additive. Giữ cả ba trong build khi còn dùng các loader này. Nếu gộp scene, phải chỉnh loader và kiểm thử tải scene đồng bộ; không chỉ xóa scene khỏi Build Settings.

## 4. Chốt bản nguồn và tách sang project đích

Đây là quy trình phối hợp đề xuất để hiện thực hóa các issue, không phải yêu cầu bổ sung từ GitHub.

1. Agent Dũng kiểm tra project nguồn/đích, `git status`, Unity version, các thay đổi local và Git LFS. Không reset hoặc ghi đè thay đổi đang có. Ghi snapshot nguồn được chọn, danh sách thay đổi local được đưa sang và snapshot project đích vào báo cáo nền.
2. Tạo bản làm việc đích riêng/branch riêng từ cùng một nền chung. Có thể dùng `codex/week2-dung` và `codex/week2-hoang`. Không suy ra `/home/dunggiait/Documents/ShadowDungeon` là Unity project đích: thư mục đó hiện chứa tài liệu, Planning và slide. Project đích cần có `Assets`, `Packages`, `ProjectSettings` hoặc được tạo ở checkout riêng phù hợp.
3. Trước khi chép, đối chiếu nội dung đích đã có. Ưu tiên đưa phần thiếu/thay đổi sang, giữ GUID và `.meta`; không đè toàn bộ Assets/ProjectSettings mù quáng. Bản clone/snapshot đầy đủ làm nền tham chiếu thường an toàn hơn copy rời các script có dependency.
4. Giữ dependency liên quan: bootstrap/DI, connection, infrastructure/pubsub, gameplay/configuration, action cả file `.Client.cs`, navigation, UI, camera, game data, prefab, material/animation/input và các asset scene tham chiếu. Giữ `.asmdef`, `.meta`, `Assets/GameData/NetworkPrefabs.asset`, `Packages/manifest.json`, `Packages/packages-lock.json` và embedded package `Packages/com.unity.multiplayer.samples.coop` khi còn tham chiếu.
5. Giữ các file giấy phép/thông báo tác giả của sample và tài nguyên được sử dụng. Không đổi namespace/tên class hàng loạt trong đợt tách; có thể làm hỏng assembly và serialized reference.
6. Không đưa `Library`, `Temp`, `Logs`, `Obj`, `UserSettings`, file IDE sinh tự động hoặc crash dump sang source bàn giao. Build xuất vào thư mục riêng và cung cấp đường dẫn artifact. Nếu Git LFS trả về pointer hoặc không checkout được asset thì phải xử lý trước khi coi nền là chạy được.
7. Mở từ Startup bằng phiên bản đã chốt, chờ import và compile. Ghi rõ Missing Script, reference null, dependency thiếu hoặc bước bị chặn; không đánh dấu pass nếu chưa chạy.
8. Dũng bàn giao nền D1, sau đó hai agent phát triển trên cùng snapshot. Chỉ loại bỏ asset/tính năng thừa khi đã xác minh không còn dependency; không cần dọn sạch toàn bộ sample để đạt tuần 2.

## 5. Gói việc của Dũng — 16 giờ

Kiểm tra chéo: Hoàng. Issue cha: [#9](https://github.com/DungGiaIT/Shadow-Dungeon/issues/9).

### D1 — Nền project, WASD, movement — 4 giờ

Nguồn: [#18 — W2-D1](https://github.com/DungGiaIT/Shadow-Dungeon/issues/18). Requirement: W2-R1. Mốc: ngày 1–2. Không ghi phụ thuộc trong issue.

- Chốt bản Unity/package và thực hiện mục 4; ghi hướng dẫn mở project đích.
- Kế thừa/hoàn thiện `ClientInputSender.cs`, `ServerCharacterMovement.cs`, input asset và cấu hình bootstrap. Camera do Hoàng sở hữu; Dũng bảo đảm dữ liệu movement/hook tương thích.
- Chỉ sửa thêm input/ServerCharacter khi cần để luồng RPC hoạt động; ghi rõ thay đổi giao diện dùng chung cho Hoàng.
- **Nghiệm thu:** project đích mở và compile; WASD hoạt động, nhả phím dừng; movement phù hợp camera Hoàng; commit/PR có dependency và `.meta` đầy đủ, kèm hướng dẫn mở bản Week 2.
- **Minh chứng:** phiên bản Unity/package chốt, log compile, clip giữ/nhả WASD sau tích hợp camera, commit hoặc PR.

### D2 — Direct IP, chọn nhân vật, tải scene — 4 giờ

Nguồn: [#19 — W2-D2](https://github.com/DungGiaIT/Shadow-Dungeon/issues/19). Requirement: W2-R3. Mốc: ngày 2–3. Phụ thuộc: D1.

- Hoàn thiện `Assets/Scripts/ConnectionManagement`, UI IP và `ServerCharSelectState.cs`; giữ flow Startup và network scene manager.
- Chạy hai phiên với hai profile khác nhau để tránh trùng định danh local. Cùng máy dùng loopback; LAN dùng IP thực của host và kiểm tra địa chỉ bind phù hợp.
- Kiểm tra khác biệt giữa luồng IP và Sessions/Relay; không buộc người dùng đi qua Relay để chạy demo local.
- **Nghiệm thu:** host/client IP kết nối, chọn nhân vật, cùng tải BossRoom, nhìn thấy nhau di chuyển; có hướng dẫn hai phiên, commit/PR và ảnh/video kết nối.
- **Minh chứng:** IP/port/profile đã dùng, thao tác chọn/ready, ảnh/video hai phiên, lỗi kết nối thực tế nếu có.

### D3 — Enemy, boss, HP, thắng/thua — 5 giờ

Nguồn: [#20 — W2-D3](https://github.com/DungGiaIT/Shadow-Dungeon/issues/20). Requirement: W2-R2, W2-R3. Mốc: ngày 3–4. Phụ thuộc: D1, D2, H2.

- Sở hữu `ServerCharacter.cs`, `Character/AI`, damage/life/health và `ServerBossRoomState.cs`, `ServerPostGameState.cs` cùng dữ liệu đồng bộ PostGame.
- Nối action của Hoàng vào damage có sẵn; giữ HP/life, AI và kết thúc trận do server quyết định. Không tạo hệ HP thứ hai chạy song song.
- Phối hợp Hoàng đặt enemy/boss, spawn và tham chiếu trong scene; Hoàng áp dụng thay đổi scene để tránh sửa đồng thời.
- **Nghiệm thu:** enemy nhận sát thương, phản công, chết đúng state; boss chết → thắng; cả đội gục → thua; hai phiên thấy cùng kết quả; có commit/PR và minh chứng cả Win lẫn Loss.
- **Minh chứng:** clip/log HP, life state, AI và hai phiên PostGame cho cả hai nhánh. Ghi lại nếu có kết thúc trùng hoặc kết quả khác nhau, chỉ sửa khi đã xác định nguyên nhân.

### D4 — Build local, kiểm thử tích hợp — 3 giờ

Nguồn: [#21 — W2-D4](https://github.com/DungGiaIT/Shadow-Dungeon/issues/21). Requirement: W2-R3, W2-Presentation. Mốc: ngày 4–5. Phụ thuộc: D2, D3, H3.

- Sở hữu `ProjectSettings/EditorBuildSettings.asset`, hướng dẫn build/run và báo cáo kỹ thuật trong `Documentation/Week2`.
- Build từ nền đã tích hợp; kiểm tra build ngoài Editor, lưu cấu hình nền tảng và commit/snapshot tương ứng.
- **Nghiệm thu:** build ngoài Editor cho host/client cùng chơi; checklist controller, combat, công tắc/cửa, thắng/thua và lỗi thực tế; có build/video/log, commit Dũng, giờ thực tế/rủi ro WBS.
- **Minh chứng:** đường dẫn bản build, phiên bản, log và bảng test mục 9. Build cũ có sẵn trong nguồn không thay thế việc xác nhận bản Week 2.

## 6. Gói việc của Hoàng — 16 giờ

Kiểm tra chéo: Dũng. Issue cha: [#8](https://github.com/DungGiaIT/Shadow-Dungeon/issues/8).

### H1 — Layout dungeon, camera — 4 giờ

Nguồn: [#14 — W2-H1](https://github.com/DungGiaIT/Shadow-Dungeon/issues/14). Requirement: W2-R1. Mốc: ngày 1–2. Phụ thuộc: D1.

- Sở hữu `Assets/Scenes/BossRoom.unity`, dungeon scene/prefab liên quan, `CameraController.cs` và prefab camera.
- Tổ chức đường từ spawn → khu combat → công tắc/cửa → boss; giữ điểm spawn hợp lệ trên NavMesh và tham chiếu additive scene. Có thể tái sử dụng layout nguồn rồi chỉnh có chủ đích.
- Dùng Cinemachine 3 theo nền Dũng đã chốt; kiểm tra mỗi phiên camera theo đúng nhân vật owner.
- **Nghiệm thu:** đường đi rõ tới combat/boss; camera theo đúng người; tường/cửa chặn đúng thiết kế; có ảnh layout, commit/PR chứa scene, prefab, `.meta`.
- **Minh chứng:** ảnh top view ghi spawn/combat/switch/door/boss và ảnh camera của cả hai phiên.

### H2 — Targeting, đánh thường, kỹ năng — 5 giờ

Nguồn: [#15 — W2-H2](https://github.com/DungGiaIT/Shadow-Dungeon/issues/15). Requirement: W2-R2. Mốc: ngày 2–3. Phụ thuộc: D1.

- Sở hữu `Assets/GameData/Action`, dữ liệu kỹ năng player trong `Assets/GameData/Character`, `Assets/Scripts/Gameplay/Action`, cấu hình CharacterClass và phản hồi combat.
- Kế thừa targeting/action request, thiết lập đòn đánh thường và ít nhất một kỹ năng đặc trưng có thể demo rõ. Bảo toàn action prototype/ID, animation và prefab hiệu ứng liên quan.
- Input chính ở `ClientInputSender.cs` do Dũng sở hữu: nếu cần thay đổi, gửi patch/yêu cầu cụ thể để tích hợp một lần. Không viết lại file này độc lập ở hai branch.
- **Nghiệm thu:** chọn mục tiêu và dùng skill trong local; đánh làm HP thay đổi, có animation hoặc hiệu ứng; commit/PR, video combat, kiểm tra chéo với damage/AI của Dũng.
- **Minh chứng:** nêu hero, action asset, thao tác thực tế và thay đổi HP nhìn được ở hai phiên. Không coi hiệu ứng hiện lên là bằng chứng damage đã chạy.

### H3 — Công tắc, cửa, physics — 3 giờ

Nguồn: [#16 — W2-H3](https://github.com/DungGiaIT/Shadow-Dungeon/issues/16). Requirement: W2-R1, W2-R2. Mốc: ngày 3–4. Phụ thuộc: H1.

- Kế thừa `FloorSwitch.cs`, `SwitchedDoor.cs` cùng prefab, cấu hình serialized references và NetworkObject/NetworkAnimator cần thiết.
- Kiểm tra trigger, Rigidbody/collider của player, layer PCs/NPCs/Ground/FloorSwitch và collision matrix thực tế. Source FloorSwitch dựa vào layer matrix để lọc đối tượng; không giả định collider nào cũng là player.
- Cửa dùng `m_PhysicsObject` chứa physics/NavMesh obstacle. Xác minh đóng/mở có hiệu lực với server movement và đồng bộ client; bake NavMesh lại khi đổi geometry.
- **Nghiệm thu:** bước lên mở cửa, rời cập nhật đúng cấu hình; cửa đóng chặn, mở đi qua; có ảnh collision/layer/trigger và commit/PR.
- **Minh chứng:** video người A giữ switch/người B qua cửa, ảnh Inspector và layer matrix, ảnh NavMesh/obstacle. Mô tả đúng hành vi “bất kỳ switch bật”; nếu đổi quy tắc phải ghi rõ lý do và kiểm thử.

### H4 — Game loop, playtest, báo cáo — 4 giờ

Nguồn: [#17 — W2-H4](https://github.com/DungGiaIT/Shadow-Dungeon/issues/17). Requirement: W2-Presentation. Mốc: ngày 4–5. Phụ thuộc: H1, H2, H3, D4.

- Sở hữu tài liệu gameplay và báo cáo tổng hợp trong `Documentation/Week2`; dùng kết quả build/test Dũng cung cấp.
- Chuẩn bị **một slide game loop**, kịch bản demo và phần nói của Hoàng; tổng hợp WBS cả hai người.
- **Nghiệm thu:** sơ đồ có `Start → Play → Challenge → Progress → Win/Game Over`; video/ảnh hai cơ chế chính và playtest thực tế; WBS đủ owner, tiến độ, giờ dự kiến/thực tế, rủi ro, thay đổi, vấn đề; cả hai có phần nói.
- **Minh chứng:** slide/sơ đồ, clip hai góc nhìn, bảng playtest và script trình bày. Agent có thể chuẩn bị kịch bản, nhưng không bịa việc hai thành viên đã thuyết trình/kiểm tra chéo.

## 7. Ranh giới hai agent và hợp đồng tích hợp

| Khu vực | Người chủ trì sửa | Phối hợp |
| --- | --- | --- |
| Unity/packages, bootstrap, connection, input, movement, Build Settings | Dũng | Hoàng dùng nền đã chốt, báo yêu cầu thay đổi |
| Camera, scene dungeon, layout, switch/door và prefab môi trường | Hoàng | Dũng cung cấp yêu cầu spawn/network object; Hoàng áp dụng vào scene |
| Action, targeting, player skill, animation/VFX | Hoàng | Giao tiếp qua RPC/action request và damage sẵn có |
| ServerCharacter, HP/life, AI, Win/Loss/PostGame | Dũng | Hoàng cấu hình action gọi damage và hỗ trợ playtest |
| Character data | Hoàng: player skill; Dũng: enemy/boss/HP | Chia đến từng asset trước khi sửa; cùng một asset chỉ một người sửa tại một thời điểm |
| NetworkPrefabs và Startup/MainMenu/CharSelect/PostGame | Dũng | Hoàng gửi danh sách prefab cần đăng ký; Dũng cập nhật registry |
| Physics/layer settings | Hoàng đề xuất/cấu hình gameplay | Do Dũng quản lý ProjectSettings nền, tích hợp patch tuần tự; không đè cả thư mục |
| Tài liệu build/test và gameplay/WBS | Dũng: build/test; Hoàng: gameplay/WBS | Dùng file riêng trong cùng thư mục tài liệu để tránh conflict |

Hợp đồng dùng chung:

- Client chỉ nhận input của owner; server xử lý movement, damage, life, switch/door và kết quả trận.
- Dùng luồng `ServerSendCharacterInputRpc`, `ServerCancelCharacterMovementRpc`, `ServerPlayActionRpc`, `ActionRequestData` và `NetworkHealthState.HitPoints` hiện có. Nếu thay đổi chữ ký, cập nhật tất cả nơi gọi trong cùng patch tích hợp.
- Không đổi GUID, ActionID, tên scene, tag camera hoặc layer tùy tiện. Duy trì reference bằng `.meta`; nếu đổi phải thống kê nơi cập nhật.
- Giữ luồng `DamageReceiver/ReceiveHitPoints → ServerCharacter → NetworkHealthState/NetworkLifeState → LifeStateChangedEventMessage → ServerBossRoomState → PostGame`.
- Tránh cùng sửa YAML scene/prefab. Người sở hữu scene áp dụng phần thay đổi cần thiết; review diff và mở lại Unity sau tích hợp.
- Nội dung sample được kế thừa phải ghi rõ trong báo cáo; commit của mỗi người nêu phần chỉnh/cấu hình/sửa/thử nghiệm thực tế.

## 8. Thứ tự thực hiện và điểm bàn giao

Phụ thuộc giữ theo issue:

```text
D1 → D2
D1 → H1 → H3
D1 → H2
D1 + D2 + H2 → D3
D2 + D3 + H3 → D4
H1 + H2 + H3 + D4 → H4
```

1. **Nền D1:** Dũng gửi Unity version, snapshot/commit, package lock, cách mở và kết quả compile. Hoàng bắt đầu cấu hình H1/H2 từ nền này; có thể đọc nguồn và chuẩn bị layout trước đó.
2. **Gameplay:** Dũng làm D2; Hoàng làm H1/H2 rồi H3. Chốt hero/skill, đường đi, trigger và enemy/boss cần đặt. H2 có thể dùng damage sẵn có để kiểm tra ban đầu; D3 tiếp nhận và kiểm thử đầy đủ.
3. **Kết quả trận:** tích hợp H2 với D3; đối chiếu HP/AI, Win/Loss ở hai phiên. Hoàng bàn giao scene/prefab H3.
4. **Build:** Dũng hoàn tất D4 sau khi tích hợp các thay đổi scene/gameplay cần thiết; Hoàng kiểm tra chéo.
5. **Báo cáo:** Hoàng hoàn tất H4 dựa trên build và playtest thực tế; Dũng cung cấp log, build, commit và giờ thực tế.

Mỗi bàn giao ghi: snapshot nguồn/đích, file thay đổi, cách chạy, kết quả đã kiểm tra, việc chưa kiểm tra và yêu cầu cho người còn lại. Các mốc ngày 1–5 là kế hoạch trong issue, không tự chuyển thành thời gian thực tế đã làm.

## 9. Checklist nghiệm thu chung và kịch bản demo

Các kết quả dưới đây hiện **chưa chạy**. Người thực hiện điền Pass/Fail/Blocked, người kiểm tra, ngày, commit/build và link minh chứng sau mỗi lần test.

| ID | Thao tác | Kết quả cần đạt | Owner chính |
| --- | --- | --- | --- |
| T01 | Mở bản đích với Unity đã chốt, import/compile từ Startup | Không thiếu dependency/script/reference cản chạy | Dũng |
| T02 | Phiên A host, B join Direct IP, chọn/ready | Hai player cùng vào BossRoom và scene additive cần thiết | Dũng |
| T03 | Hai người WASD, giữ/nhả, đi chéo, focus UI | Di chuyển/dừng đúng, không điều khiển nhầm; camera mỗi phiên đúng owner | Dũng + Hoàng |
| T04 | Đi từ spawn qua tường/hành lang tới combat/boss | Layout rõ, không xuyên tường/kẹt do NavMesh | Hoàng |
| T05 | Chọn target, đánh thường, dùng ít nhất một skill | HP giảm, animation/VFX phản hồi; cả hai phiên thấy thống nhất | Hoàng + Dũng |
| T06 | Giao chiến enemy | Enemy phản công, HP/life và chết đúng, đồng bộ | Dũng |
| T07 | A đứng switch, B đi qua cửa; A rời switch | Mở/đóng đúng cấu hình; cửa đóng chặn, mở đi qua; đồng bộ | Hoàng |
| T08 | Đánh bại boss | Sau delay dự kiến cả hai tới PostGame Win | Dũng |
| T09 | Bắt đầu lượt khác, để toàn đội gục | Cả hai tới PostGame Loss | Dũng |
| T10 | Chạy build ngoài Editor với phiên thứ hai | Lặp được T02, T03, T05, T07, T08, T09; không chỉ hoạt động trong Editor | Dũng, Hoàng kiểm tra chéo |
| T11 | Đối chiếu báo cáo và artifacts | Hai cơ chế, hai góc nhìn, game loop, WBS, commit của cả hai và phần nói đầy đủ | Hoàng |

Hướng dẫn chạy đề xuất, phải được Dũng xác nhận trên build cuối:

1. Mở hai instance hoặc Editor + một build; dùng hai profile khác nhau. Vào **START WITH DIRECT IP**.
2. Cùng máy: A Host tại `127.0.0.1`, port `9998`; B Join cùng IP/port. Nếu UI đã thay mặc định, ghi lại giá trị dùng thực tế.
3. Hai máy LAN: host dùng địa chỉ LAN có thể bind; client nhập IP LAN của host và cùng port. Không dùng loopback của client để kết nối máy khác. Ghi điều kiện mạng/firewall nếu ảnh hưởng test.
4. Cả hai chọn nhân vật/Ready, kiểm tra nhìn thấy nhau và camera riêng. Quay đồng thời hai góc nhìn hoặc hai clip có điểm đồng bộ rõ.
5. Demo cơ chế 1: targeting/combat/skill và HP/AI; cơ chế 2: người A giữ công tắc, B qua cửa; chỉ ra Collider/Trigger/layer/NavMesh thực tế.
6. Kết thúc một lượt bằng thắng và một lượt riêng bằng thua. Giữ log và ảnh PostGame của cả hai.

## 10. Sản phẩm phải bàn giao

Gợi ý tên file cho các agent triển khai sau; các file này **chưa được tạo bởi tài liệu bàn giao này**:

```text
Documentation/Week2/
  WEEK2_AGENT_HANDOFF.md       # File hiện tại, dùng chung
  DUNG_IMPLEMENTATION.md      # Nền, dependency, thay đổi, build/run
  HOANG_IMPLEMENTATION.md     # Layout, camera, skill, physics, demo
  INTEGRATION_TESTS.md        # Test thực tế và lỗi, Dũng chủ trì
  WBS.md                      # Hoàng tổng hợp từ giờ thực tế hai người
  GAME_LOOP.md                # Nội dung/sơ đồ cho một slide
  Evidence/                   # Ảnh, log hoặc chỉ mục link video/build
```

- Project Week 2 mở/compile được, source/asset/`.meta`/dependency đủ; có commit/PR gắn từng task và ghi phần kế thừa từ sample.
- Build chạy ngoài Editor, hướng dẫn host/join, phiên bản Unity/nền tảng/commit được ghi nhận.
- Ảnh layout/camera; video hai cơ chế và hai góc nhìn; minh chứng Win và Loss; ảnh Collider/Trigger/layer/NavMesh.
- Một slide game loop và phần nói của cả hai: Dũng trình bày nền/input/network/AI/kết quả/build; Hoàng trình bày layout/camera/combat/switch/physics/game loop.
- Báo cáo kiểm thử có lỗi thực tế, cách tái hiện, trạng thái sửa và giới hạn chưa kiểm chứng. Nếu không có Unity hoặc công cụ quay/build, ghi Blocked và bước người dùng cần chạy; không tạo minh chứng giả.

Mẫu WBS, điền theo từng task (thay dấu “—” sau khi thực hiện):

| Task | Issue | Owner | Dự kiến | Thực tế | Trạng thái | Commit/PR | Test/minh chứng | Kiểm tra chéo | Rủi ro, thay đổi, vấn đề |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| D1 | #18 | Dũng | 4h | — | Chưa xác nhận | — | — | Hoàng | — |
| D2 | #19 | Dũng | 4h | — | Chưa xác nhận | — | — | Hoàng | — |
| D3 | #20 | Dũng | 5h | — | Chưa xác nhận | — | — | Hoàng | — |
| D4 | #21 | Dũng | 3h | — | Chưa xác nhận | — | — | Hoàng | — |
| H1 | #14 | Hoàng | 4h | — | Chưa xác nhận | — | — | Dũng | — |
| H2 | #15 | Hoàng | 5h | — | Chưa xác nhận | — | — | Dũng | — |
| H3 | #16 | Hoàng | 3h | — | Chưa xác nhận | — | — | Dũng | — |
| H4 | #17 | Hoàng | 4h | — | Chưa xác nhận | — | — | Dũng | — |

Giải thích chênh lệch giữa giờ thực tế và ước tính trong từng task. Issue cha #8/#9 chỉ tổng hợp, không tính giờ lần hai. Không tạo thêm tầng sub-issue bên dưới các task con: cả 8 issue đã quy định chúng là tầng cuối.

## 11. Prompt gửi trực tiếp cho từng agent

### Prompt dành cho agent của Dũng

```text
Bạn triển khai phần Week 2 của Nguyễn Tiến Dũng cho Shadow Dungeon, dựa trên
DemoDungeon. Đọc toàn bộ WEEK2_AGENT_HANDOFF.md đính kèm, đặc biệt mục 3–5 và 7–10.
Thực hiện issue #18, #19, #20, #21 thuộc issue cha #9, milestone 2 của
https://github.com/DungGiaIT/Shadow-Dungeon. Chỉ triển khai phạm vi Week 2.

Trước khi sửa, xác định thư mục nguồn DemoDungeon và Unity project đích thực tế,
đọc hướng dẫn repository, kiểm tra Git status và bảo toàn thay đổi đang có.
Nguồn local từng có Unity 6000.5.8f1 trong ProjectVersion nhưng README ghi
6000.0.52f1: chốt nền compile được, ghi snapshot cùng thay đổi local đưa sang.
Không tự giả định HEAD là toàn bộ bản nguồn. So sánh project đích trước khi copy.

Kế thừa hệ thống sẵn có, bảo toàn GUID/.meta/.asmdef/embedded package/dependency.
Làm nền Unity, input WASD, movement, Direct IP, chọn nhân vật, tải scene,
HP/life/AI, thắng/thua và build/test tích hợp. Port IP mặc định nguồn là 9998.
Giữ mô hình server quyết định trạng thái. Dùng mục 5 làm tiêu chí từng task.

Hoàng sở hữu layout/camera/action/switch/door. Bạn sở hữu input, ServerCharacter,
connection, nền ProjectSettings và registry NetworkPrefabs; phối hợp theo mục 7.
Bàn giao nền D1 và hợp đồng damage/action để Hoàng triển khai, rồi tích hợp
H2/H3 trước khi nghiệm thu D3/D4. Chỉ một người sửa cùng scene/asset mỗi lúc.

Hoàn thiện source và tài liệu DUNG_IMPLEMENTATION.md, INTEGRATION_TESTS.md;
cung cấp giờ thực tế, commit/PR và bằng chứng cho WBS của Hoàng. Chạy kiểm thử
hai phiên và build ngoài Editor nếu môi trường cho phép; ghi rõ Pass/Fail/Blocked.
Không báo hoàn thành dựa vào việc script đã tồn tại. Không bịa log/video/giờ làm.
Không tự đăng comment, đóng issue hoặc push thay đổi lên GitHub khi chưa được yêu cầu.
Cuối cùng báo file đã thay đổi, cách chạy, test đã chạy, bằng chứng và việc còn chặn.
```

### Prompt dành cho agent của Hoàng

```text
Bạn triển khai phần Week 2 của Phạm Huy Hoàng cho Shadow Dungeon, dựa trên
DemoDungeon. Đọc toàn bộ WEEK2_AGENT_HANDOFF.md đính kèm, đặc biệt mục 3–4 và 6–10.
Thực hiện issue #14, #15, #16, #17 thuộc issue cha #8, milestone 2 của
https://github.com/DungGiaIT/Shadow-Dungeon. Chỉ triển khai phạm vi Week 2.

Xác định nguồn và Unity project đích, đọc hướng dẫn repository, bảo toàn thay đổi
local và dùng cùng nền D1 do Dũng bàn giao. Có thể đọc/chuẩn bị trước, nhưng
không tự nâng package hoặc dùng nền Unity khác. Nguồn có mismatch README/version;
ghi rõ dependency hoặc bản nền chưa có, không đoán đã compile.

Kế thừa layout/prefab/scene, Cinemachine 3, action/CharacterClass, floor switch/door;
giữ GUID/.meta/.asmdef và asset tham chiếu. Làm đường spawn → combat → boss,
camera owner, targeting/đánh thường/ít nhất một skill, công tắc/cửa và physics/
NavMesh. Dùng mục 6 làm tiêu chí nghiệm thu từng task. Giữ đúng hành vi switch
nguồn: bất kỳ switch liên kết bật thì cửa mở, rời hết thì đóng, trừ khi có thay đổi
được thống nhất và ghi nhận rõ.

Bạn sở hữu scene/camera/action/switch/door. Dũng sở hữu input, ServerCharacter,
HP/life/AI, network, nền packages/settings, registry và build. Dùng hợp đồng mục 7;
nếu cần input/HP/ProjectSettings/NetworkPrefabs, bàn giao patch hoặc yêu cầu cụ thể.
Áp dụng yêu cầu đặt enemy/boss vào scene do bạn quản lý, tránh hai agent sửa YAML
scene/asset cùng lúc. Không tạo hệ HP hoặc network thứ hai.

Hoàn thiện HOANG_IMPLEMENTATION.md, game loop cho một slide, kịch bản demo hai
cơ chế/hai góc nhìn, tổng hợp WBS và phần nói cả hai. H4 cần kết quả build D4:
kiểm tra chéo trên build tích hợp, ghi playtest thực tế, giờ thực tế và bằng chứng.
Không bịa video, kết quả test hoặc nhận xét đã được Dũng xác nhận.
Không tự đăng comment, đóng issue hoặc push thay đổi lên GitHub khi chưa được yêu cầu.
Cuối cùng báo file thay đổi, cấu hình scene/physics/skill, test đã chạy,
commit/PR hoặc patch, bằng chứng và những phụ thuộc còn thiếu.
```

Hai prompt này là nội dung để người dùng gửi sau. Việc tạo tài liệu không đồng nghĩa đã khởi chạy hai agent hoặc triển khai gameplay.
