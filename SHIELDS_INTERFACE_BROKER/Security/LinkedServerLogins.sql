EXECUTE sp_addlinkedsrvlogin @rmtsrvname = N'SHCSECTRADB', @useself = N'FALSE', @rmtuser = N'ReadingRadPooledUser';


GO
EXECUTE sp_addlinkedsrvlogin @rmtsrvname = N'SHCSCTDB', @useself = N'FALSE', @rmtuser = N'ReadingRadPooledUser';


GO
EXECUTE sp_addlinkedsrvlogin @rmtsrvname = N'172.31.100.176', @useself = N'FALSE', @rmtuser = N'ReadingRadPooledUser';

