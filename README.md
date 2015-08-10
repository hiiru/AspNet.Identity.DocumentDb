# AspNet.Identity.DocumentDb

This will be a simple AspNet Identity 3 DocumentDb provider.

**Note: this code currently has no unit tests, use at your own risk!**


This Code has some limitations:
- Only works for the full CLR (no DocumentDb support for CoreCLR in the offical library, nor is any source available)
- Sub-Optimal way to handle it (a DocumentDb integration in EF7 with the AspNet.Identity.EntityFramework provider might be better, because then only one provider codebase is used)
- No Tests yet!

