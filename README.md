<h1 align="center">Simple: Message Board</h1>

<div align="center">
  Simple Console App emulating a message board - Inlogik Technical Test
</div>

<br />

<img alt="Sample Screenshot" src="https://github.com/user-attachments/assets/9b455952-9ce9-4e59-bdd0-f393a2aaa84e" />

<br />

<div align="center">
    <!-- License -->
    <a href="https://choosealicense.com/licenses/mit/">
    <img src="https://img.shields.io/badge/License-MIT-blue.svg?style=flat-square" alt="License - MIT" />
    </a>

</div>


---
## Overview

## General requirements

- Application must be a **C#** console application only using .Net 6+ (no web/database)
- User submits commands to the application:
  - posting: `<user name> -> @<project name> <message>`
  - reading: `<project name>`
  - following: `<user name> follows <project name>`
  - wall: `<user name> wall`
- Don't worry about handling any exceptions or invalid commands. Assume that the user will always type the correct commands. Just focus on the sunny day scenarios.
- **NOTE:** "posting:", "reading:", "following:" and "wall:" are not part of the command. All commands start with the user name, except for reading which starts with the project name.
- **IMPORTANT:** Implement the requirements focusing on **writing the best code** you can produce implementing **CQRS** and **SOLID** principles

## Technology used

- Mediatr
- OneOf
- FluentValidation
- Moq
- xUnit
- Spectre Console


## AI Used
- GitHub copilot (model: Claude Sonnet 4)
  - writing this Readme (didn't like the scenario examples, though)
  - Test data generation (e.g. MemberData)
  - Validators
  - Creating the Spectre Console UI and parsing

## Notes
  - No validation in the InMemory 'database'
  - Didn't get to the the WALL command
  
## Scenarios

In the below scenarios `">"` denotes the start of user input command in the scenarios listed above and is not part of the input.

**Posting**: Alice and Bob can publish messages to project Moonshot and Apollo's timeline

> `>` Alice -> @Moonshot I'm working on the log on screen  
> `>` Bob -> @Moonshot Awesome, I'll start on the forgotten password screen  
> `>` Bob -> @Apollo Has anyone thought about the next release demo?  
> `>` Bob -> @Moonshot I'll give you the link to put on the log on screen shortly Alice  

**Reading**: Bob can view a projects Moonshot's timeline

> `>` Moonshot  
> Alice  
> I'm working on the login screen (5 minutes ago)  
> Bob  
> Awesome, I'll start on the forgotten password screen (4 minutes ago)  
> I'll give you the link to put on the log on screen shortly Alice (1 minute ago)

**Following**: Charlie can subscribe to Moonshot's and Apollo's timelines, and view an aggregated list of all subscriptions

> `>` Charlie follows Apollo  
> `>` Charlie wall  
> Apollo - Bob: Has anyone thought about the next release demo? (6 minutes ago)  
> `>` Charlie follows Moonshot  
> `>` Charlie wall  
> Moonshot - Alice: I'm working on the log on screen (9 minutes ago)  
> Moonshot - Bob: Awesome, I'll start on the forgotten password screen (8 minutes ago)  
> Apollo - Bob: Has anyone thought about the next release demo? (7 minutes ago)  
> Moonshot - Bob: I'll give you the link to put on the log on screen shortly Alice (5 minutes ago)  
---
