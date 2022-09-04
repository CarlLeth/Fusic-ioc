cd dist
nuget pack ../src/Fusic/Fusic.csproj -properties Configuration=Release -symbols
nuget pack ../src/Fusic.AspNet/Fusic.AspNet.csproj -properties Configuration=Release -symbols
