---
layout: article
sidebar:
    nav: devs_cn
---

# 事件类型

## MiniParse 数据统计

### CombatData 战斗数据

当玩家战斗时，每秒发送一次此事件。<br>
在悬浮窗中加载自带文件 `miniparse_debug.html` ，并查看它的源代码来获得关于此事件类型的完整结构演示。

### LogLine 日志行

每个日志行产生时发送此事件。使用网络日志行格式，即每部分数据使用 `|` 分割。

字段 | 描述
------|--------------
`line`|`Array(String 数据1, String 数据2, ...)` 包含一个分割处理过的日志行数据数组。
`rawLine`|`String 日志行` 包含未经处理的日志行字符串，采用网络日志行格式。

### ImportedLogLines 导入日志行

在导入日志时，每秒发送一次此事件。

字段 | 描述
------|--------------
`logLines`|`Array(String 日志行)` 一个包含每个日志行字符串的数组。

### ChangeZone 区域变更

每当玩家登录或移动到一个新的区域或副本时发送此事件。

字段 | 描述
------|--------------
`zoneID`|`Int 区域ID` 新区域的ID。

### ChangePrimaryPlayer 当前玩家变更

每当玩家登录或玩家变化时发送此事件。

字段 | 描述
------|--------------
`charID`|`String 角色ID` 玩家的实体ID
`charName`|`String 角色名` 玩家的角色名字

### OnlineStatusChanged 在线状态变更

每当玩家或附近角色的在线状态发生变化时发送此事件。

字段 | 描述
------|--------------
`target`|`String 目标` 状态变更所属目标的实体ID
`rawStatus`|`Int 原始状态代码` 状态代码(例如`12`)
`status`|`String 状态文本` 状态的描述字符串。可能的值为: `Online, Busy, InCutscene, AFK, LookingToMeld, RP, LookingForParty`

### PartyChanged 小队变更

每次小队成员发生变化时或小队进入新的区域时发送此事件。

该事件只有一个字段 `party` ，

`Array(Array 小队成员1, Array 小队成员2, ...)` 其中包含小队成员名单。

每个的字段解释如下。

字段 | 描述
------|--------------
`id`|`String 实体ID` 实体ID
`name`|`String 角色名` 角色名
`worldId`|`Int 世界ID` 原始世界ID
`job`|`Int 职业ID` 职业ID
`inParty`|`Bool true` 如果这个角色在玩家的队伍中的话肯定为True（

### BroadcastMessage 广播消息

每当任何悬浮窗调用 `broadcast` callOverlayHandler 方法时发送此事件。

字段 | 描述
------|--------------
`source`|`String 来源` 表明来源的字符串。
`msg`|`[Object] 消息正文` 消息正文内容。

示例:
```js
callOverlayHandler({
	call: 'broadcast',
	source: 'testOverlay',
	msg: {
		oneKey: 'test',
		someOther: 'key',
		anyValid: ['json', 'value', 123],
	},
});
````
