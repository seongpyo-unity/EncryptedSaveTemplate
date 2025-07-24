# 🔐 Encrypted Save Template

AES 암호화를 기반으로 한 Unity 세이브/로드 템플릿
로컬 저장 데이터를 안전하게 보호하고, 손상된 세이브에 대한 복구 기능 제공


## ✨ 주요 기능

- 🔒 AES-256 암호화 (세이브 파일마다 IV 사용)
- 🧪 손상된 저장 파일 자동 복구 (백업 시스템)
- 🎯 제네릭 기반의 범용 저장 구조
- 📂 멀티 슬롯 저장 기능
- 🔧 실무 적용 가능한 미니멀 구조




## 🛡 암호화 세부 사항

|       항목       |                         설명                          |
|-----------------|-------------------------------------------------|
| 암호화 방식   | AES-256 (CBC 모드)                              |
| IV 처리         | 저장 파일마다 개별 생성 및 함께 저장됨   |
| 키 보관 방식  | 메모리 내 유지                                     |
| 백업 파일      |  .bak 자동 생성하여 복구에 사용됨          |

> ⚠ `GameDataBase`를 반드시 상속해야 함



## 🧪 테스트 도구

- Unity 에디터에서 테스트 가능한 `SimpleSaveUI` 포함



## 📌 요구 사항

- Unity **2022.3.6f1** 이상
- .NET Standard 2.1 호환 환경



## 📜 라이선스

MIT License © 2025 seongpyo-unity

This project is licensed under the MIT License.

Note: Some class implementations in this project were inspired by or adapted from the following instructional content:

- Source: James Doyle  
  “Unity 2D Dungeon Gunner Roguelike Development Course”  
  (https://www.udemy.com/course/unity-2d-dungeon-gunner-roguelike-development-course/)

The referenced code was originally intended for educational use and has been modified for integration into this project.  
All copyrights for the course materials remain with the original author.