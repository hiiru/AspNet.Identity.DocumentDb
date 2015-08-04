# AspNet.Identity.DocumentDb

This will be a simple AspNet Identity 3 DocumentDb provider.

**Note: this code is currently not working and still a untested work in progress!**


This Code has some limitations:
- Only works for the full CLR (no DocumentDb support for CoreCLR in the offical library, nor any source availalbe)
- Sub-Optimal way to handle it (a DocumentDb integration in EF7 with the AspNet.Identity.EntityFramework provider might be better, because then only one provider codebase is used)
- No Tests yet!

