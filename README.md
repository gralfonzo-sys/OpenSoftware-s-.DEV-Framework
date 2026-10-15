# .DEV Framework (C++)

Reescritura del .DEV Framework en C++ con CMake.

Estructura:
- src/core: núcleo (IPlugin, PluginLoader)
- src/host: host de ejemplo que carga plugins desde carpeta "plugins"
- samples/sample_plugin: plugin de ejemplo (biblioteca compartida)
- tests: pruebas locales usando Catch2

Requisitos:
- CMake 3.16+
- Compilador con soporte C++17
- (Opcional) Qt si activas USE_QT en CMake

Compilar:
  mkdir build && cd build
  cmake ..
  cmake --build . --config Release

Ejecutar host:
  ./src/host/DevFrameworkHost  (o la ruta correspondiente en Windows)

Nota: compila el sample_plugin y coloca su DLL/.so en la carpeta plugins del ejecutable host.

License: Apache-2.0
