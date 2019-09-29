module.exports = {
    // ����Ӧ��ʱ�Ļ��� URL
    publicPath: './',
    // buildʱ�����ļ���Ŀ¼ ����ʱ���� --no-clean �ɹرո���Ϊ
    outputDir: 'dist',
    // buildʱ�������ɵľ�̬��Դ (js��css��img��fonts) �� (����� outputDir ��) Ŀ¼
    assetsDir: 'static',
    // ָ�����ɵ� index.html �����·�� (����� outputDir)��Ҳ������һ������·����
    indexPath: 'index.html',
    // Ĭ�������ɵľ�̬��Դ�ļ����а���hash�Կ��ƻ���
    filenameHashing: true,
    // �Ƿ�ʹ�ð�������ʱ�������� Vue �����汾
    runtimeCompiler: false,
    // Babel ��ʽת���б�
    transpileDependencies: [],
    // ����㲻��Ҫ���������� source map�����Խ�������Ϊ false �Լ���������������
    productionSourceMap: false,
    // �������ɵ� HTML �� <link rel="stylesheet"> �� <script> ��ǩ�� crossorigin ���ԣ�ע����Ӱ�칹��ʱע��ı�ǩ��

    crossorigin: '',
    // �����ɵ� HTML �е� <link rel="stylesheet"> �� <script> ��ǩ������ Subresource Integrity (SRI)
    integrity: false,
    // ������ֵ��һ���������ͨ�� webpack-merge �ϲ������յ�������
    // �������Ҫ���ڻ�����������������Ϊ��������Ҫֱ���޸����ã��Ǿͻ���һ������ (�ú������ڻ�������������֮����ִ��)���÷����ĵ�һ���������յ��Ѿ������õ����á��ں����ڣ������ֱ���޸����ã����߷���һ�����ᱻ�ϲ��Ķ���
    configureWebpack: {},
    // ���ڲ��� webpack ���ã������޸ġ�����Loaderѡ�(��ʽ����)
    chainWebpack: () => { },
    // css�Ĵ���
    css: {
        // ��Ϊtrueʱ��css�ļ�����ʡ�� module Ĭ��Ϊ false
        modules: true,
        // �Ƿ�����е� CSS ��ȡ��һ�������� CSS �ļ���,����Ϊһ���⹹��ʱ����Ҳ���Խ�������Ϊ false ����û��Լ����� CSS
        // Ĭ�������������� true�������������� false
        extract: true,
        // �Ƿ�Ϊ CSS ���� source map������Ϊ true ֮����ܻ�Ӱ�칹��������
        sourceMap: false,
        //�� CSS ��ص� loader ����ѡ��(֧�� css-loader postcss-loader sass-loader less-loader stylus-loader)
        loaderOptions: { css: 'css-loader', less: {} }
    },
    // ���� webpack-dev-server ��ѡ�֧��
    devServer: {
        open: true,
        host: '0.0.0.0',
        port: 12345,
        https: false,
        hotOnly: false,
        proxy: null,
        // proxy: {
        //     '/api': {
        //         target: '<url>',
        //         ws: true,
        //         changOrigin: true
        //     }
        // },
        before: app => { }
    },
    // �Ƿ�Ϊ Babel �� TypeScript ʹ�� thread-loader
    parallel: require('os').cpus().length > 1,
    // �� PWA �������ѡ��
    pwa: {},
    // �������������κε��������ѡ��
    pluginOptions: {}
}