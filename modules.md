## Modules

Most contents inside a module should be `internal` or `private` to ensure encapsulation.  
This prevents unwanted coupling between modules.

The only **allowed cross-module interaction** is through the **Contracts layer**.  
Each module should be self-contained and unaware of others.

Contracts implementation is provided inside `[Module Name] > Integrations`.