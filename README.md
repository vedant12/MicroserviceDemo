# MicroserviceDemo

A demo project showcasing a microservices architecture using .NET / C#.  
It contains multiple services (Auth, Product, Coupon, Shopping Cart) and a web frontend.

## Table of Contents

- [Overview](#overview)  
- [Architecture](#architecture)  
- [Project Structure](#project-structure)  
- [Technologies](#technologies)  
- [Getting Started](#getting-started)  
  - [Prerequisites](#prerequisites)  
  - [Setup & Run](#setup--run)  
- [Usage](#usage)  
- [Testing](#testing)  
- [Contributing](#contributing)  
- [License](#license)  

---

## Overview

This repository demonstrates a microservices-based application (coded in C# / .NET) for an e-commerce-like scenario.  
It includes:

- **Auth API** – handles authentication & user management  
- **Product API** – handles product catalog  
- **Coupon API** – manages coupons & discounts  
- **Shopping Cart API** – manages shopping cart operations  
- **Web Frontend** – user-facing UI to interact with these services  

The goal is to illustrate how multiple independent services communicate, and how a frontend aggregates them.

## Architecture

Each service is built as an independent microservice. They may communicate synchronously (e.g. HTTP / REST) or asynchronously (message queues, etc.).  
The Web Frontend interacts with the microservices and presents a unified UI.

You can extend this design with API Gateway, service discovery, circuit-breakers, or message brokers.

## Project Structure

/
├── Mango.Services.AuthAPI
├── Mango.Services.ProductAPI
├── Mango.Services.CouponAPI
├── Mango.Services.ShoppingCartAPI
├── Mango.Web
├── Mango.slnx
├── .gitignore
└── .gitattributes


- **Mango.Services.AuthAPI** – Authentication / user management microservice  
- **Mango.Services.ProductAPI** – Product catalog microservice  
- **Mango.Services.CouponAPI** – Coupon / discount microservice  
- **Mango.Services.ShoppingCartAPI** – Shopping cart microservice  
- **Mango.Web** – Web application (frontend)  

## Technologies

- **.NET / C#**  
- Web API / REST  
- (Optional) Entity Framework / ORM (if database used)  
- Web frontend (Razor pages / MVC / SPA)  
- JSON / HTTP for communication between services  

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/) (version compatible with the project)  
- IDE (e.g. Visual Studio, VS Code)  
- (Optional) SQL Server / SQLite / other DB if services require a database  
- (Optional) Docker, if you want to containerize the services  

### Setup & Run

1. **Clone the repo**

   ```bash
   git clone https://github.com/vedant12/MicroserviceDemo.git
   cd MicroserviceDemo
dotnet restore
dotnet build
