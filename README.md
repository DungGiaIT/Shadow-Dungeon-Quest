<p align="center">
  <img src="docs/assets/shadow-dungeon-logo.svg" alt="Shadow Dungeon logo" width="300">
</p>

<h1 align="center">Shadow Dungeon</h1>

<p align="center">
  <strong>Enter the dark. Face the dungeon together.</strong><br>
  Prototype game co-op khám phá hầm ngục, chiến đấu và phối hợp để đánh bại boss.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/Unity-6000.0.52f1-222222?logo=unity" alt="Unity 6000.0.52f1">
  <img src="https://img.shields.io/badge/Render%20Pipeline-URP-7b68ee" alt="URP">
  <img src="https://img.shields.io/badge/Status-In%20development-f0ad4e" alt="In development">
</p>

## Giới thiệu

**Shadow Dungeon** là prototype game dungeon crawler co-op đang được phát triển bằng Unity. Người chơi sẽ khám phá các khu vực trong hầm ngục, phối hợp kỹ năng, vượt qua các đợt quái và đối đầu với boss.

Repo hiện đang ở giai đoạn khởi tạo: có cấu hình Unity URP, scene mẫu và Input System actions. Các hệ thống gameplay, multiplayer, UI và nội dung màn chơi đang nằm trong roadmap; những mục này chưa được xem là hoàn thành chỉ dựa trên issue hoặc commit mô phỏng.

## Định hướng gameplay

- Điều khiển nhân vật trong góc nhìn top-down.
- Chọn hero với vai trò và kỹ năng khác nhau.
- Chiến đấu với quái, vượt encounter và tiến tới boss.
- Chơi cùng bạn bè qua kết nối trực tiếp hoặc session online.
- Theo dõi trạng thái đội, mục tiêu và kết quả trận qua HUD.

> Đây là mục tiêu thiết kế. Tính năng chỉ được đánh dấu hoàn thành khi đã được triển khai và kiểm tra trong game.

## Trạng thái hiện tại

| Hạng mục | Trạng thái |
| --- | --- |
| Unity project và cấu hình URP | Có trong repo |
| Scene mẫu | `Assets/Scenes/SampleScene.unity` |
| Input System actions | `Assets/InputSystem_Actions.inputactions` |
| Gameplay, AI và combat | Đang lên kế hoạch |
| Multiplayer và Relay | Đang lên kế hoạch |
| HUD, menu và nội dung dungeon | Đang lên kế hoạch |
| Build phát hành | Chưa có |

## Bắt đầu

### Yêu cầu

- Unity **6000.0.52f1**.
- Git.
- Kết nối mạng lần đầu để Unity khôi phục các package trong `Packages/manifest.json`.

### Mở project

1. Clone repository:

   ```bash
   git clone https://github.com/DungGiaIT/Shadow-Dungeon.git
   cd Shadow-Dungeon
   ```

2. Thêm project trong Unity Hub và chọn Unity `6000.0.52f1`.
3. Mở scene `Assets/Scenes/SampleScene.unity`.
4. Chờ Unity hoàn tất import package và asset.

### Build

Project chưa có bản build được xác nhận. Khi cần tạo build thử nghiệm, mở **File → Build Profiles**, chọn nền tảng, thêm scene cần thiết vào danh sách build rồi build từ Unity Editor. Ghi lại phiên bản Editor, nền tảng và commit nguồn cùng artifact.

## Điều khiển dự kiến

Các phím dưới đây là mapping dự kiến trong kế hoạch, chưa được xác nhận là đã hoạt động trong build hiện tại.

| Thao tác | Phím dự kiến |
| --- | --- |
| Di chuyển | `W`, `A`, `S`, `D` |
| Ngắm / chọn mục tiêu | Chuột |
| Kỹ năng | Phím số hoặc phím được gán theo hero |
| Bật thông tin mạng | `X` |

## Roadmap

Các hạng mục phát triển và tiêu chí nghiệm thu được theo dõi trong [GitHub Issues](https://github.com/DungGiaIT/Shadow-Dungeon/issues).

- **Core gameplay:** điều khiển, chọn nhân vật, combat, HP, thắng/thua và chơi lại.
- **Dungeon systems:** encounter, wave spawning, AI và tiến trình tới boss.
- **Co-op:** kết nối, đồng bộ gameplay, trạng thái phiên và xử lý mất kết nối.
- **Polish:** HUD, menu, hiệu ứng, âm thanh, cân bằng và tài liệu demo.

## Cấu trúc project

```text
Assets/
├── Scenes/                 # Scene Unity
├── Settings/               # URP render pipeline và volume settings
├── InputSystem_Actions.inputactions
└── TutorialInfo/           # Tài nguyên hướng dẫn đi kèm Unity template
Packages/                   # Package manifest và lockfile
ProjectSettings/            # Cấu hình project và phiên bản Unity
```

## Tài nguyên và ghi công

Repo hiện dùng cấu hình và tài nguyên hướng dẫn có sẵn trong Unity URP template. Nguồn của asset bên thứ ba, âm thanh, hình ảnh và công cụ hỗ trợ sẽ được ghi tại đây khi được đưa vào project.

Các issue hiện giao phần việc theo kế hoạch cho [DungGiaIT](https://github.com/DungGiaIT) và [Hoang271205](https://github.com/Hoang271205). Bảng đóng góp thực tế sẽ được cập nhật dựa trên tính năng đã hoàn thành và được kiểm tra.

## Báo lỗi và đóng góp

Mở issue để mô tả bước tái hiện, kết quả mong đợi, kết quả thực tế và ảnh/video/log liên quan. Pull request nên nêu rõ issue liên quan, phạm vi thay đổi và cách xác minh.

## License

Repo hiện chưa có file `LICENSE`. Vui lòng liên hệ chủ sở hữu repository trước khi sử dụng lại hoặc phân phối nội dung của project.
